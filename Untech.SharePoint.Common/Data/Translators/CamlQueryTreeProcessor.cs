using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Data.Translators.Predicate;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal class CamlQueryTreeProcessor : ExpressionVisitor, IExpressionProcessor<Expression>
	{
		public CamlQueryTreeProcessor()
		{
			CombineRules = new Dictionary<MethodInfo, ICallCombineRule> (new GenericMethodDefinitionComparer())
			{
				{OpUtils.SpqFakeGetAll, new InitContextRule()},
				{OpUtils.QWhere, new WhereCallCombineRule()},
				{OpUtils.QAny, new AnyCallCombineRule()},
				{OpUtils.QAnyP, new AnyCallCombineRule()},
				{OpUtils.QAll, new AllCallCombineRule()},

				{OpUtils.QOrderBy, new OrderByCallCombineRule { ResetOrder = true, Ascending = true}},
				{OpUtils.QOrderByDescending, new OrderByCallCombineRule{ ResetOrder = true }},
				{OpUtils.QThenBy, new OrderByCallCombineRule {Ascending = true}},
				{OpUtils.QThenrByDescending, new OrderByCallCombineRule()},

				{OpUtils.QTake, new TakeCallCombineRule()},
				{OpUtils.QSkip, new SkipCallCombineRule()},

				{OpUtils.QSingle, new FirstCallCombineRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
				{OpUtils.QSingleOrDefault, new FirstCallCombineRule {ThrowIfMultiple = true}},

				{OpUtils.QSingleP, new FirstCallCombineRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
				{OpUtils.QSingleOrDefaultP, new FirstCallCombineRule {ThrowIfMultiple = true}},

				{OpUtils.QFirst, new FirstCallCombineRule {ThrowIfNothing = true}},
				{OpUtils.QFirstOrDefault, new FirstCallCombineRule()},

				{OpUtils.QFirstP, new FirstCallCombineRule {ThrowIfNothing = true}},
				{OpUtils.QFirstOrDefaultP, new FirstCallCombineRule()},

				{OpUtils.QLast, new LastRewriteRule {ThrowIfNothing = true}},
				{OpUtils.QLastOrDefault, new LastRewriteRule()},

				{OpUtils.QLastP, new LastRewriteRule {ThrowIfNothing = true}},
				{OpUtils.QLastOrDefaultP, new LastRewriteRule()},

				{OpUtils.QElementAt, new ElementAtRewriteRule {ThrowIfNothing = true}},
				{OpUtils.QElementAtOrDefault, new ElementAtRewriteRule()},

				{OpUtils.QReverse, new ReverseRewriteRule()},

				{OpUtils.QCount, new CountRewriteRule()}
			};
		}

		protected IReadOnlyDictionary<MethodInfo, ICallCombineRule> CombineRules { get; set; }
		protected HashSet<Expression> Candidates { get; private set; }

		public Expression Process(Expression node)
		{
			Candidates = CallCombineNominator.GetCandidates(CombineRules, node);

			return Visit(node);
		}

		public override Expression Visit(Expression node)
		{
			if (node.NodeType == ExpressionType.Call && Candidates.Contains(node))
			{
				return new SubtreeCallsCombiner(CombineRules).Visit(node);
			}
			if (node.NodeType == ExpressionType.Call)
			{
				return VisitMethodCall((MethodCallExpression)node);
			}
			throw InvalidChain(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof (Queryable))
			{
				var newSource = Visit(node.Arguments[0]);

				var args = new []{ newSource }.Concat(node.Arguments.Skip(1));

				return node.Update(null, args);
			}

			return node;
		}

		private static Exception InvalidChain(Expression node)
		{
			throw new NotSupportedException(string.Format("Expression: '{0}' is not supported", node));
		}

		#region [Nested Classes]

		internal class SubtreeCallsCombiner : ExpressionVisitor, ICallsCombinerContext
		{
			public SubtreeCallsCombiner(IReadOnlyDictionary<MethodInfo, ICallCombineRule> combineRules)
			{
				CombineRules = combineRules;
			}

			public ISpItemsProvider ItemsProvider { get; set; }
			public QueryModel Query { get; set; }
			protected IReadOnlyDictionary<MethodInfo, ICallCombineRule> CombineRules { get; set; }
			
			public override Expression Visit(Expression node)
			{
				switch (node.NodeType)
				{
					case ExpressionType.Call:
						return VisitMethodCall((MethodCallExpression)node);
					case ExpressionType.Quote:
						return VisitUnary((UnaryExpression)node);
				}
				return node;
			}

			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				var rule = GetRuleAndUpdateContext(node);
				return rule.Combine(this, node);
			}

			protected ICallCombineRule GetRuleAndUpdateContext(Expression node)
			{
				if (node.NodeType == ExpressionType.Call)
				{
					return GetRuleAndUpdateContext((MethodCallExpression)node);
				}
				throw InvalidChain(node);
			}

			protected ICallCombineRule GetRuleAndUpdateContext(MethodCallExpression node)
			{
				if (node.Method.DeclaringType == typeof(Queryable))
				{
					GetRuleAndUpdateContext(node.Arguments[0]);
				}

				if (!CombineRules.ContainsKey(node.Method))
				{
					throw InvalidChain(node);
				}

				var currentRule = CombineRules[node.Method];

				currentRule.UpdateContext(this, node);

				return currentRule;
			}
		}

		internal class CallCombineNominator : ExpressionVisitor
		{
			public CallCombineNominator(IReadOnlyDictionary<MethodInfo, ICallCombineRule> combineRules)
			{
				Candidates = new HashSet<Expression>();
				CombineRules = combineRules;
			}

			public static HashSet<Expression> GetCandidates(IReadOnlyDictionary<MethodInfo, ICallCombineRule> builderParts, Expression node)
			{
				var nominator = new CallCombineNominator(builderParts);
				nominator.Visit(node);
				return nominator.Candidates;
			}
			
			public HashSet<Expression> Candidates { get; private set; }

			protected IReadOnlyDictionary<MethodInfo, ICallCombineRule> CombineRules { get; set; }
			protected bool OuterCallCombineAllowed { get; set; }

			public override Expression Visit(Expression node)
			{
				switch (node.NodeType)
				{
					case ExpressionType.Call:
						return VisitMethodCall((MethodCallExpression)node);
					case ExpressionType.Quote:
						return VisitUnary((UnaryExpression)node);
				}
				return node;
			}

			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				if (node.Method.DeclaringType == typeof(Queryable))
				{
					Visit(node.Arguments[0]);
				}

				if (OpUtils.IsOperator(OpUtils.SpqFakeGetAll, node.Method))
				{
					OuterCallCombineAllowed = true;
				}
				if (CombineRules.ContainsKey(node.Method))
				{
					if (OuterCallCombineAllowed)
					{
						Candidates.Add(node);
					}
					OuterCallCombineAllowed &= CombineRules[node.Method].OuterCallCombineAllowed;
				}
				else
				{
					OuterCallCombineAllowed = false;
				}

				return node;
			}
		}


		internal interface ICallsCombinerContext
		{
			QueryModel Query { get; set; }

			ISpItemsProvider ItemsProvider { get; set; }
		}

		internal interface ICallCombineRule
		{
			bool OuterCallCombineAllowed { get; }

			void UpdateContext(ICallsCombinerContext context, MethodCallExpression node);

			Expression Combine(ICallsCombinerContext context, MethodCallExpression node);
		}

		internal class InitContextRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return true; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				context.Query = new QueryModel();
				context.ItemsProvider = (ISpItemsProvider) ((ConstantExpression) node.Arguments[0].StripQuotes()).Value;
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeGetAll(entityType, context.ItemsProvider, context.Query));
			}
		}

		internal class WhereCallCombineRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return true; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];

				context.Query.MergeWheres(new CamlPredicateProcessor().Process(predicate));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeGetAll(entityType, context.ItemsProvider, context.Query));
			}
		}

		internal class AnyCallCombineRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				if (node.Arguments.Count != 2)
				{
					return;
				}

				var predicate = node.Arguments[1];

				context.Query.MergeWheres(new CamlPredicateProcessor().Process(predicate));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeAny(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query);
			}
		}

		internal class AllCallCombineRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];

				context.Query.MergeWheres(new CamlPredicateProcessor().Process(predicate).Negate());
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeAny(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query);
			}
		}

		internal class OrderByCallCombineRule : ICallCombineRule
		{
			public bool Ascending { get; set; }

			public bool ResetOrder { get; set; }

			public bool OuterCallCombineAllowed
			{
				get { return true; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				if (ResetOrder)
				{
					context.Query.ResetOrder();
				}

				context.Query.MergeOrderBys(new OrderByModel(new CamlKeySelectorProcessor().Process(node.Arguments[1]), Ascending));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeGetAll(entityType, context.ItemsProvider, context.Query));
			}
		}

		internal class TakeCallCombineRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				context.Query.RowLimit = (int) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value;
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeTake(entityType, context.ItemsProvider, context.Query));
			}
		}

		internal class SkipCallCombineRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{

			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var count = (int) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value;

				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeSkip(entityType, context.ItemsProvider, context.Query, count));
			}
		}

		internal class FirstCallCombineRule : ICallCombineRule
		{
			public bool ThrowIfNothing { get; set; }

			public bool ThrowIfMultiple { get; set; }

			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				if (node.Arguments.Count != 2)
				{
					return;
				}

				var predicate = node.Arguments[1];

				context.Query.MergeWheres(new CamlPredicateProcessor().Process(predicate));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeFirst(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query,
					ThrowIfNothing, ThrowIfMultiple);
			}
		}

		internal class LastRewriteRule : ICallCombineRule
		{
			public bool ThrowIfNothing { get; set; }

			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				context.Query.ReverseOrder();

				if (node.Arguments.Count != 2)
				{
					return;
				}

				var predicate = node.Arguments[1];

				context.Query.MergeWheres(new CamlPredicateProcessor().Process(predicate));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeFirst(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query,
					ThrowIfNothing, false);
			}
		}

		internal class ElementAtRewriteRule : ICallCombineRule
		{
			public bool ThrowIfNothing { get; set; }

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var count = (int) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value;

				return SpQueryable.MakeElementAt(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query, count,
					ThrowIfNothing);
			}

			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{

			}
		}

		internal class ReverseRewriteRule : ICallCombineRule
		{
			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeGetAll(entityType, context.ItemsProvider, context.Query));
			}

			public bool OuterCallCombineAllowed
			{
				get { return true; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				context.Query.ReverseOrder();
			}
		}

		internal class CountRewriteRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{

			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeCount(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query);
			}
		}

		
		#endregion
	}

}