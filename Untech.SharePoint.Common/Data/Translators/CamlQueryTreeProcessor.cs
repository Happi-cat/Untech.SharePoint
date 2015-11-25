using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Data.Translators.Predicate;
using Untech.SharePoint.Common.Diagnostics;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal sealed class CamlQueryTreeProcessor : ExpressionVisitor, IProcessor<Expression, Expression>
	{
		[NotNull]
		private static readonly IReadOnlyDictionary<MethodInfo, ICallCombineRule> CombineRules;

		static CamlQueryTreeProcessor()
		{
			CombineRules = new Dictionary<MethodInfo, ICallCombineRule>(new GenericMethodDefinitionComparer())
			{
				{MethodUtils.SpqFakeFetch, new InitContextRule()},

				{MethodUtils.QSelect, new SelectCallCombineRule()},

				{MethodUtils.QMinP, new MinPCallCombineRule()},
				{MethodUtils.QMaxP, new MaxPCallCombineRule()},

				{MethodUtils.QWhere, new WhereCallCombineRule()},
				{MethodUtils.QAny, new AnyCallCombineRule()},
				{MethodUtils.QAnyP, new AnyCallCombineRule()},
				{MethodUtils.QAll, new AllCallCombineRule()},

				{MethodUtils.QOrderBy, new OrderByCallCombineRule {ResetOrder = true, Ascending = true}},
				{MethodUtils.QOrderByDescending, new OrderByCallCombineRule {ResetOrder = true}},
				{MethodUtils.QThenBy, new OrderByCallCombineRule {Ascending = true}},
				{MethodUtils.QThenrByDescending, new OrderByCallCombineRule()},

				{MethodUtils.QTake, new TakeCallCombineRule()},
				{MethodUtils.QSkip, new SkipCallCombineRule()},

				{MethodUtils.QSingle, new FirstCallCombineRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
				{MethodUtils.QSingleOrDefault, new FirstCallCombineRule {ThrowIfMultiple = true}},

				{MethodUtils.QSingleP, new FirstCallCombineRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
				{MethodUtils.QSingleOrDefaultP, new FirstCallCombineRule {ThrowIfMultiple = true}},

				{MethodUtils.QFirst, new FirstCallCombineRule {ThrowIfNothing = true}},
				{MethodUtils.QFirstOrDefault, new FirstCallCombineRule()},

				{MethodUtils.QFirstP, new FirstCallCombineRule {ThrowIfNothing = true}},
				{MethodUtils.QFirstOrDefaultP, new FirstCallCombineRule()},

				{MethodUtils.QLast, new LastRewriteRule {ThrowIfNothing = true}},
				{MethodUtils.QLastOrDefault, new LastRewriteRule()},

				{MethodUtils.QLastP, new LastRewriteRule {ThrowIfNothing = true}},
				{MethodUtils.QLastOrDefaultP, new LastRewriteRule()},

				{MethodUtils.QElementAt, new ElementAtRewriteRule {ThrowIfNothing = true}},
				{MethodUtils.QElementAtOrDefault, new ElementAtRewriteRule()},

				{MethodUtils.QReverse, new ReverseRewriteRule()},

				{MethodUtils.QCount, new CountRewriteRule()}
			};
		}

		private HashSet<Expression> Candidates { get; set; }

		public Expression Process([NotNull]Expression node)
		{
			Logger.Debug(LogCategories.QueryTreeProcessor, "Original expressions tree:\n{0}", node);

			Candidates = CallCombineNominator.GetCandidates(node);

			var result = Visit(node);

			Logger.Debug(LogCategories.QueryTreeProcessor, "Rewritten expressions tree:\n{0}", result);

			return result;
		}

		public override Expression Visit(Expression node)
		{
			if (node.NodeType == ExpressionType.Call && Candidates.Contains(node))
			{
				return new SubtreeCallsCombiner().Visit(node);
			}
			if (node.NodeType == ExpressionType.Call)
			{
				return VisitMethodCall((MethodCallExpression)node);
			}
			throw Error.SubqueryNotSupported(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType != typeof (Queryable))
			{
				return node;
			}

			var newSource = Visit(node.Arguments[0]);

			var args = new []{ newSource }.Concat(node.Arguments.Skip(1));

			return node.Update(null, args);
		}


		#region [Nested Classes]

		private class SubtreeCallsCombiner : ExpressionVisitor, ICallsCombinerContext
		{
			public ISpListItemsProvider ListItemsProvider { get; set; }
			public QueryModel Query { get; set; }
			
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

			private ICallCombineRule GetRuleAndUpdateContext(Expression node)
			{
				if (node.NodeType == ExpressionType.Call)
				{
					return GetRuleAndUpdateContext((MethodCallExpression)node);
				}
				throw Error.SubqueryNotSupported(node);
			}

			private ICallCombineRule GetRuleAndUpdateContext(MethodCallExpression node)
			{
				if (node.Method.DeclaringType == typeof(Queryable))
				{
					GetRuleAndUpdateContext(node.Arguments[0]);
				}

				if (!CombineRules.ContainsKey(node.Method))
				{
					throw Error.SubqueryNotSupported(node);
				}

				var currentRule = CombineRules[node.Method];

				currentRule.UpdateContext(this, node);

				return currentRule;
			}
		}

		private class CallCombineNominator : ExpressionVisitor
		{
			private CallCombineNominator()
			{
				Candidates = new HashSet<Expression>();
			}

			public static HashSet<Expression> GetCandidates(Expression node)
			{
				var nominator = new CallCombineNominator();
				nominator.Visit(node);
				return nominator.Candidates;
			}

			private HashSet<Expression> Candidates { get; set; }

			private bool OuterCallCombineAllowed { get; set; }

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

				if (MethodUtils.IsOperator(MethodUtils.SpqFakeFetch, node.Method))
				{
					OuterCallCombineAllowed = true;
					Candidates.Add(node);
					return node;
				}

				if (CombineRules.ContainsKey(node.Method))
				{
					if (OuterCallCombineAllowed)
					{
						Candidates.Add(node);
					}
					var rule = CombineRules[node.Method];
					OuterCallCombineAllowed &= rule.OuterCallCombineAllowed;
				}
				else
				{
					OuterCallCombineAllowed = false;
				}

				return node;
			}
		}


		private interface ICallsCombinerContext
		{
			QueryModel Query { get; set; }

			ISpListItemsProvider ListItemsProvider { get; set; }
		}

		private interface ICallCombineRule
		{
			bool OuterCallCombineAllowed { get; }

			void UpdateContext(ICallsCombinerContext context, MethodCallExpression node);

			Expression Combine(ICallsCombinerContext context, MethodCallExpression node);
		}

		private class InitContextRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return true; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				context.Query = new QueryModel();
				context.ListItemsProvider = (ISpListItemsProvider) ((ConstantExpression) node.Arguments[0].StripQuotes()).Value;
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query));
			}
		}

		private class WhereCallCombineRule : ICallCombineRule
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
					SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query));
			}
		}

		private class SelectCallCombineRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];

				context.Query.MergeSelectableFields(new CamlSelectableFieldsProcessor().Process(predicate));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var genericArgs = node.Method.GetGenericArguments();

				var lambdaNode = (LambdaExpression) node.Arguments[1].StripQuotes();

				return SpQueryable.MakeAsQueryable(genericArgs[1],
					SpQueryable.MakeSelect(genericArgs[0], genericArgs[1], context.ListItemsProvider, context.Query, lambdaNode));
			}
		}

		private class MinPCallCombineRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];

				context.Query.MergeSelectableFields(new CamlSelectableFieldsProcessor().Process(predicate));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var genericArgs = node.Method.GetGenericArguments();

				var lambdaNode = (LambdaExpression)node.Arguments[1].StripQuotes();

				return SpQueryable.MakeMin(genericArgs[0], genericArgs[1], context.ListItemsProvider, context.Query, lambdaNode);
			}
		}

		private class MaxPCallCombineRule : ICallCombineRule
		{
			public bool OuterCallCombineAllowed
			{
				get { return false; }
			}

			public void UpdateContext(ICallsCombinerContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];

				context.Query.MergeSelectableFields(new CamlSelectableFieldsProcessor().Process(predicate));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var genericArgs = node.Method.GetGenericArguments();

				var lambdaNode = (LambdaExpression)node.Arguments[1].StripQuotes();

				return SpQueryable.MakeMax(genericArgs[0], genericArgs[1], context.ListItemsProvider, context.Query, lambdaNode);
			}
		}

		private class AnyCallCombineRule : ICallCombineRule
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
				return SpQueryable.MakeAny(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query);
			}
		}

		private class AllCallCombineRule : ICallCombineRule
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
				return SpQueryable.MakeAny(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query);
			}
		}

		private class OrderByCallCombineRule : ICallCombineRule
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

				context.Query.MergeOrderBys(new OrderByModel(new CamlFieldSelectorProcessor().Process(node.Arguments[1]), Ascending));
			}

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query));
			}
		}

		private class TakeCallCombineRule : ICallCombineRule
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
					SpQueryable.MakeTake(entityType, context.ListItemsProvider, context.Query));
			}
		}

		private class SkipCallCombineRule : ICallCombineRule
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
					SpQueryable.MakeSkip(entityType, context.ListItemsProvider, context.Query, count));
			}
		}

		private class FirstCallCombineRule : ICallCombineRule
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
				return SpQueryable.MakeFirst(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query,
					ThrowIfNothing, ThrowIfMultiple);
			}
		}

		private class LastRewriteRule : ICallCombineRule
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
				return SpQueryable.MakeFirst(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query,
					ThrowIfNothing, false);
			}
		}

		private class ElementAtRewriteRule : ICallCombineRule
		{
			public bool ThrowIfNothing { get; set; }

			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var count = (int) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value;

				return SpQueryable.MakeElementAt(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query, count,
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

		private class ReverseRewriteRule : ICallCombineRule
		{
			public Expression Combine(ICallsCombinerContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType,
					SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query));
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

		private class CountRewriteRule : ICallCombineRule
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
				return SpQueryable.MakeCount(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query);
			}
		}

		
		#endregion
	}

}