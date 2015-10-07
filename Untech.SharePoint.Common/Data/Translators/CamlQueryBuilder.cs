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
	public class CamlQueryBuilder : IExpressionProcessor<Expression>
	{
		public interface ICamlQueryBuilderPart
		{
			bool IsOuterCallBuildAllowed(MethodCallExpression node);

			void UpdateContext(BuilderContext context, MethodCallExpression node);

			Expression Build(BuilderContext context, MethodCallExpression node);
		}

		public class BuilderContext
		{
			public QueryModel Query { get; set; }

			public ISpItemsProvider ItemsProvider { get; set; }
		}

		internal class BuilderNominator : ExpressionVisitor
		{
			public BuilderNominator(IReadOnlyDictionary<MethodInfo, ICamlQueryBuilderPart> rules)
			{
				Candidates = new HashSet<Expression>();
				Rules = rules;
			}


			public HashSet<Expression> Candidates { get; private set; }
			public IReadOnlyDictionary<MethodInfo, ICamlQueryBuilderPart> Rules { get; set; }
			protected bool CanBeOuterCallBuilt { get; set; }

			public override Expression Visit(Expression node)
			{
				if (node.NodeType == ExpressionType.Call)
				{
					return VisitMethodCall((MethodCallExpression)node);
				}
				if (node.NodeType == ExpressionType.Quote)
				{
					return VisitUnary((UnaryExpression) node);
				}
				return node;
			}

			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				if (node.Method.DeclaringType == typeof(Queryable))
				{
					Visit(node.Arguments[0]);
				}

				var method = node.Method;
				if (method.IsGenericMethod)
				{
					method = method.GetGenericMethodDefinition();
				}

				if (OpUtils.SpqFakeGetAll == method)
				{
					CanBeOuterCallBuilt = true;
				}
				if (Rules.ContainsKey(method))
				{
					if (CanBeOuterCallBuilt)
					{
						Candidates.Add(node);
					}
					CanBeOuterCallBuilt &= Rules[method].IsOuterCallBuildAllowed(node);
				}
				else
				{
					CanBeOuterCallBuilt = false;
				}

				return node;
			}
		}

		public CamlQueryBuilder()
		{
			Rules = new Dictionary<MethodInfo, ICamlQueryBuilderPart>
			{
				{OpUtils.SpqFakeGetAll, new InitBuilderPart()},
				{OpUtils.QWhere, new WhereBuilderPart()},
				{OpUtils.QAny, new AnyBuilderPart()},
				{OpUtils.QAnyP, new AnyBuilderPart()},
				{OpUtils.QAll, new AllBuilderPart()},

				{OpUtils.QOrderBy, new OrderByBuilderPart { ResetOrder = true, Ascending = true}},
				{OpUtils.QOrderByDescending, new OrderByBuilderPart{ ResetOrder = true }},
				{OpUtils.QThenBy, new OrderByBuilderPart {Ascending = true}},
				{OpUtils.QThenrByDescending, new OrderByBuilderPart()},

				{OpUtils.QTake, new TakeBuilderPart()},
				{OpUtils.QSkip, new SkipBuilderPart()},

				{OpUtils.QSingle, new FirstBuilderPart {ThrowIfMultiple = true, ThrowIfNothing = true}},
				{OpUtils.QSingleOrDefault, new FirstBuilderPart {ThrowIfMultiple = true}},

				{OpUtils.QFirst, new FirstBuilderPart {ThrowIfNothing = true}},
				{OpUtils.QFirstOrDefault, new FirstBuilderPart()},

				{OpUtils.QLast, new LastRewriteRule {ThrowIfNothing = true}},
				{OpUtils.QLastOrDefault, new LastRewriteRule()},

				{OpUtils.QElementAt, new ElementAtRewriteRule {ThrowIfNothing = true}},
				{OpUtils.QElementAtOrDefault, new ElementAtRewriteRule()},

				{OpUtils.QReverse, new ReverseRewriteRule()},

				{OpUtils.QCount, new CountRewriteRule()}
			};

			Context = new BuilderContext();
		}

		protected IReadOnlyDictionary<MethodInfo, ICamlQueryBuilderPart> Rules { get; set; }
		public HashSet<Expression> Candidates { get; private set; }

		protected BuilderContext Context { get; set; }


		public Expression Process(Expression node)
		{
			var nominator = new BuilderNominator(Rules);
			nominator.Visit(node);
			Candidates = nominator.Candidates;

			return VisitAndBuild(node);
		}


		public Expression VisitAndBuild(Expression node)
		{
			if (node.NodeType == ExpressionType.Call && Candidates.Contains(node))
			{
				var rule = VisitBuilderMethodCall((MethodCallExpression)node);
				return rule.Build(Context, (MethodCallExpression)node);
			}
			if (node.NodeType == ExpressionType.Call)
			{
				return VisitMethodCall((MethodCallExpression)node);
			}
			throw new ArgumentException();
		}

		protected Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof (Queryable))
			{
				var newSource = VisitAndBuild(node.Arguments[0]);

				var args = new []{ newSource }.Concat(node.Arguments.Skip(1));

				return node.Update(null, args);
			}

			return node;
		}

		public ICamlQueryBuilderPart VisitBuilder(Expression node)
		{
			if (node.NodeType == ExpressionType.Call)
			{
				return VisitBuilderMethodCall((MethodCallExpression)node);
			}
			throw new ArgumentException();
		}

		protected ICamlQueryBuilderPart VisitBuilderMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof(Queryable))
			{
				VisitBuilder(node.Arguments[0]);
			}

			var method = node.Method;
			if (method.IsGenericMethod)
			{
				method = method.GetGenericMethodDefinition();
			}

			if (Rules.ContainsKey(method))
			{
				var currentRule = Rules[method];

				currentRule.UpdateContext(Context, node);

				return currentRule;
			}

			throw new NotSupportedException();
		}

		protected class InitBuilderPart : ICamlQueryBuilderPart
		{
			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return true;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{
				context.Query = new QueryModel();
				context.ItemsProvider = (ISpItemsProvider)((ConstantExpression)node.Arguments[0].StripQuotes()).Value;
			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType, SpQueryable.MakeGetAll(entityType, context.ItemsProvider, context.Query));
			}
		}

		protected class WhereBuilderPart : ICamlQueryBuilderPart
		{
			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return true;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];

				context.Query.MergeWheres(new CamlPredicateProcessor().Process(predicate));
			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType, SpQueryable.MakeGetAll(entityType, context.ItemsProvider, context.Query));
			}
		}

		protected class AnyBuilderPart : ICamlQueryBuilderPart
		{
			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return false;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{
				if (node.Arguments.Count != 2)
				{
					return;
				}

				var predicate = node.Arguments[1];

				context.Query.MergeWheres(new CamlPredicateProcessor().Process(predicate));
			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeAny(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query);
			}
		}

		protected class AllBuilderPart : ICamlQueryBuilderPart
		{
			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return false;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];

				context.Query.MergeWheres(new CamlPredicateProcessor().Process(predicate).Negate());
			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeAny(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query);
			}
		}

		protected class OrderByBuilderPart : ICamlQueryBuilderPart
		{
			public bool Ascending { get; set; }

			public bool ResetOrder { get; set; }

			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return true;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{
				if (ResetOrder)
				{
					context.Query.ResetOrder();
				}

				context.Query.MergeOrderBys(new OrderByModel(new CamlKeySelectorProcessor().Process(node.Arguments[1]), Ascending));
			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType, SpQueryable.MakeGetAll(entityType, context.ItemsProvider, context.Query));
			}
		}

		protected class TakeBuilderPart : ICamlQueryBuilderPart
		{
			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return false;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{
				context.Query.RowLimit = (int)((ConstantExpression)node.Arguments[1].StripQuotes()).Value;
			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType, SpQueryable.MakeTake(entityType, context.ItemsProvider, context.Query));
			}
		}

		protected class SkipBuilderPart : ICamlQueryBuilderPart
		{
			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return false;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{

			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				var count = (int)((ConstantExpression)node.Arguments[1].StripQuotes()).Value;

				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType, SpQueryable.MakeSkip(entityType, context.ItemsProvider, context.Query, count));
			}
		}

		protected class FirstBuilderPart : ICamlQueryBuilderPart
		{
			public bool ThrowIfNothing { get; set; }

			public bool ThrowIfMultiple { get; set; }

			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return false;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{

			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeFirst(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query, ThrowIfNothing, ThrowIfMultiple);
			}
		}

		protected class LastRewriteRule : ICamlQueryBuilderPart
		{
			public bool ThrowIfNothing { get; set; }

			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return false;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{
				context.Query.ReverseOrder();
			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeFirst(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query, ThrowIfNothing, false);
			}
		}

		protected class ElementAtRewriteRule : ICamlQueryBuilderPart
		{
			public bool ThrowIfNothing { get; set; }

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				var count = (int)((ConstantExpression)node.Arguments[1].StripQuotes()).Value;

				return SpQueryable.MakeElementAt(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query, count, ThrowIfNothing);
			}

			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return false;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{

			}
		}

		protected class ReverseRewriteRule : ICamlQueryBuilderPart
		{
			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeAsQueryable(entityType, SpQueryable.MakeGetAll(entityType, context.ItemsProvider, context.Query));
			}

			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return true;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{
				context.Query.ReverseOrder();
			}
		}

		protected class CountRewriteRule : ICamlQueryBuilderPart
		{
			public bool IsOuterCallBuildAllowed(MethodCallExpression node)
			{
				return false;
			}

			public void UpdateContext(BuilderContext context, MethodCallExpression node)
			{

			}

			public Expression Build(BuilderContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeCount(node.Method.GetGenericArguments()[0], context.ItemsProvider, context.Query);
			}
		}

	}
}