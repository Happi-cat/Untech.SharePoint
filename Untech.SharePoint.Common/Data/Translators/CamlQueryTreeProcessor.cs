using System;
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
	internal sealed class CamlQueryTreeProcessor : IProcessor<Expression, Expression>
	{
		[NotNull]
		private static readonly IReadOnlyDictionary<MethodInfo, ICombineRule> CombineRules;

		static CamlQueryTreeProcessor()
		{
			CombineRules = new Dictionary<MethodInfo, ICombineRule>(new GenericMethodDefinitionComparer())
			{
				{MethodUtils.SpqFakeFetch, new InitContextRule()},

				{MethodUtils.QSelect, new SelectCombineRule()},

				{MethodUtils.QMinP, new MinPCombineRule()},
				{MethodUtils.QMaxP, new MaxPCombineRule()},

				{MethodUtils.QWhere, new WhereCombineRule()},
				{MethodUtils.QAny, new AnyCombineRule()},
				{MethodUtils.QAnyP, new AnyCombineRule()},
				{MethodUtils.QAll, new AllCombineRule()},

				{MethodUtils.QOrderBy, new OrderByCombineRule {ResetOrder = true, Ascending = true}},
				{MethodUtils.QOrderByDescending, new OrderByCombineRule {ResetOrder = true}},
				{MethodUtils.QThenBy, new OrderByCombineRule {Ascending = true}},
				{MethodUtils.QThenrByDescending, new OrderByCombineRule()},

				{MethodUtils.QTake, new TakeCombineRule()},

				{MethodUtils.QSingle, new FirstCombineRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
				{MethodUtils.QSingleP, new FirstCombineRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
				{MethodUtils.QSingleOrDefault, new FirstCombineRule {ThrowIfMultiple = true}},
				{MethodUtils.QSingleOrDefaultP, new FirstCombineRule {ThrowIfMultiple = true}},

				{MethodUtils.QFirst, new FirstCombineRule {ThrowIfNothing = true}},
				{MethodUtils.QFirstP, new FirstCombineRule {ThrowIfNothing = true}},
				{MethodUtils.QFirstOrDefault, new FirstCombineRule()},
				{MethodUtils.QFirstOrDefaultP, new FirstCombineRule()},

				{MethodUtils.QLast, new LastRewriteRule {ThrowIfNothing = true}},
				{MethodUtils.QLastP, new LastRewriteRule {ThrowIfNothing = true}},
				{MethodUtils.QLastOrDefault, new LastRewriteRule()},
				{MethodUtils.QLastOrDefaultP, new LastRewriteRule()},

				{MethodUtils.QElementAt, new ElementAtRewriteRule {ThrowIfNothing = true}},
				{MethodUtils.QElementAtOrDefault, new ElementAtRewriteRule()},

				{MethodUtils.QReverse, new ReverseRewriteRule()},

				{MethodUtils.QCount, new CountRewriteRule()}
			};
		}

		public Expression Process([NotNull]Expression node)
		{
			Logger.Debug(LogCategories.QueryTreeProcessor, "Original expressions tree:\n{0}", node);

			var result = new SubtreeCallsCombiner().Visit(node);

			Logger.Debug(LogCategories.QueryTreeProcessor, "Rewritten expressions tree:\n{0}", result);

			return result;
		}

		#region [Nested Classes]

		private class SubtreeCallsCombiner : ExpressionVisitor
		{
			[NotNull] private readonly RuleContext _context = new RuleContext();
			
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
				return rule.Get(_context, node);
			}

			private ICombineRule GetRuleAndUpdateContext(Expression node)
			{
				switch (node.NodeType)
				{
					case ExpressionType.Call:
						return GetRuleAndUpdateContext((MethodCallExpression)node);
					case ExpressionType.Quote:
						return GetRuleAndUpdateContext(((UnaryExpression)node).Operand);
				}
				throw Error.SubqueryNotSupported(node);
			}

			private ICombineRule GetRuleAndUpdateContext(MethodCallExpression node)
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
				
				ThrowIfCannotApplyAfterProjection(currentRule, node);
				ThrowIfCannotApplyAfterRowLimit(currentRule, node);

				currentRule.Apply(_context, node);

				return currentRule;
			}

			private void ThrowIfCannotApplyAfterProjection(ICombineRule rule, MethodCallExpression node)
			{
				if (!_context.ProjectionApplied) return;
				if (rule.CanApplyAfterProjection(node)) return;

				throw  Error.SubqueryNotSupportedAfterProjection(node);
			}

			private void ThrowIfCannotApplyAfterRowLimit(ICombineRule rule, MethodCallExpression node)
			{
				if (!_context.RowLimitApplied) return;
				if (rule.CanApplyAfterRowLimit(node)) return;

				throw Error.SubqueryNotSupportedAfterRowLimit(node);
			}
		}

		private class RuleContext
		{
			public QueryModel Query { get; set; }

			public ISpListItemsProvider ListItemsProvider { get; set; }

			public Type EntityType { get; private set; }

			public LambdaExpression Projection { get; private set; }

			public Type ProjectedType { get; private set; }

			public bool ProjectionApplied { get; private set; }
			
			public bool RowLimitApplied { get; private set; }

			public void ApplyContentType(Type entityType)
			{
				EntityType = entityType;
			}

			public void ApplyFiltering(Expression predicate, bool negate = false)
			{
				var where = new CamlPredicateProcessor().Process(predicate);
				if (negate)
				{
					where = where.Negate();
				}
				Query.MergeWheres(where);
			}

			public void ApplyOrdering(Expression predicate, bool ascending)
			{
				Query.MergeOrderBys(new OrderByModel(new CamlFieldSelectorProcessor().Process(predicate), ascending));
			}

			public void ApplyProjection(Expression predicate)
			{
				Query.MergeSelectableFields(new CamlSelectableFieldsProcessor().Process(predicate));

				Projection = (LambdaExpression)predicate.StripQuotes();
				ProjectedType = Projection.ReturnType;
				ProjectionApplied = true;
			}

			public void ApplyRowLimit(int rowLimit)
			{
				Query.RowLimit = rowLimit;
				RowLimitApplied = true;
			}
		}

		private interface ICombineRule
		{
			bool CanApplyAfterProjection(MethodCallExpression node);

			bool CanApplyAfterRowLimit(MethodCallExpression node);

			void Apply(RuleContext context, MethodCallExpression node);

			Expression Get(RuleContext context, MethodCallExpression node);
		}

		private class InitContextRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return false;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return false;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				context.Query = new QueryModel();
				context.ListItemsProvider = (ISpListItemsProvider) ((ConstantExpression) node.Arguments[0].StripQuotes()).Value;

				var entityType = node.Method.GetGenericArguments()[0];
				context.ApplyContentType(entityType);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query);
			}
		}

		private class WhereCombineRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return false;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return false;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];
				context.ApplyFiltering(predicate);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query);
			}
		}

		private class SelectCombineRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return false;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return true;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];
				context.ApplyProjection(predicate);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				var genericArgs = node.Method.GetGenericArguments();

				var lambdaNode = (LambdaExpression) node.Arguments[1].StripQuotes();

				return SpQueryable.MakeSelect(genericArgs[0], genericArgs[1], context.ListItemsProvider, context.Query, lambdaNode);
			}
		}

		private class MinPCombineRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return false;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return true;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];
				context.ApplyProjection(predicate);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				var genericArgs = node.Method.GetGenericArguments();

				var lambdaNode = (LambdaExpression)node.Arguments[1].StripQuotes();

				return SpQueryable.MakeMin(genericArgs[0], genericArgs[1], context.ListItemsProvider, context.Query, lambdaNode);
			}
		}

		private class MaxPCombineRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return false;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return true;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];
				context.ApplyProjection(predicate);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				var genericArgs = node.Method.GetGenericArguments();

				var lambdaNode = (LambdaExpression)node.Arguments[1].StripQuotes();

				return SpQueryable.MakeMax(genericArgs[0], genericArgs[1], context.ListItemsProvider, context.Query, lambdaNode);
			}
		}

		private class AnyCombineRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return node.Arguments.Count == 1;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return node.Arguments.Count == 1;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				if (node.Arguments.Count != 2)
				{
					return;
				}

				var predicate = node.Arguments[1];
				context.ApplyFiltering(predicate);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeAny(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query);
			}
		}

		private class AllCombineRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return false;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return false;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				var predicate = node.Arguments[1];
				context.ApplyFiltering(predicate, true);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeAny(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query);
			}
		}

		private class OrderByCombineRule : ICombineRule
		{
			public bool Ascending { private get; set; }

			public bool ResetOrder { private get; set; }

			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return false;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return false;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				if (ResetOrder)
				{
					context.Query.ResetOrder();
				}
				context.ApplyOrdering(node.Arguments[1], Ascending);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query);
			}
		}

		private class TakeCombineRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return true;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return false;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				context.ApplyRowLimit((int) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				if (context.ProjectionApplied)
				{
					return SpQueryable.MakeTake(context.EntityType, context.ProjectedType, context.ListItemsProvider, context.Query, context.Projection);
				}

				return SpQueryable.MakeTake(context.EntityType, context.ListItemsProvider, context.Query);
			}
		}

		private class FirstCombineRule : ICombineRule
		{
			public bool ThrowIfNothing { private get; set; }

			public bool ThrowIfMultiple { private get; set; }

			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return node.Arguments.Count == 1;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return node.Arguments.Count == 1;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				context.ApplyRowLimit(1);

				if (node.Arguments.Count != 2)
				{
					return;
				}

				var predicate = node.Arguments[1];
				context.ApplyFiltering(predicate);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				if (context.ProjectionApplied)
				{
					return SpQueryable.MakeFirst(context.EntityType, context.ProjectedType, context.ListItemsProvider, context.Query,
						ThrowIfNothing, ThrowIfMultiple, context.Projection);
				}

				return SpQueryable.MakeFirst(context.EntityType, context.ListItemsProvider, context.Query,
					ThrowIfNothing, ThrowIfMultiple);
			}
		}

		private class LastRewriteRule : ICombineRule
		{
			public bool ThrowIfNothing { private get; set; }

			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return node.Arguments.Count == 1;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return false;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				context.Query.ReverseOrder();
				context.ApplyRowLimit(1);

				if (node.Arguments.Count != 2)
				{
					return;
				}

				var predicate = node.Arguments[1];
				context.ApplyFiltering(predicate);
			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				if (context.ProjectionApplied)
				{
					return SpQueryable.MakeFirst(context.EntityType, context.ProjectedType, context.ListItemsProvider, context.Query,
						ThrowIfNothing, false, context.Projection);
				}

				return SpQueryable.MakeFirst(context.EntityType, context.ListItemsProvider, context.Query,
					ThrowIfNothing, false);
			}
		}

		private class ElementAtRewriteRule : ICombineRule
		{
			public bool ThrowIfNothing { private get; set; }

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				var count = (int) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value;

				if (context.ProjectionApplied)
				{
					return SpQueryable.MakeElementAt(context.EntityType, context.ProjectedType, context.ListItemsProvider, context.Query, count,
						ThrowIfNothing, context.Projection);
				}

				return SpQueryable.MakeElementAt(context.EntityType, context.ListItemsProvider, context.Query, count,
					ThrowIfNothing);
			}

			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return true;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return false;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{

			}
		}

		private class ReverseRewriteRule : ICombineRule
		{
			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				var entityType = node.Method.GetGenericArguments()[0];
				return SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query);
			}

			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return true;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return false;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{
				context.Query.ReverseOrder();
			}
		}

		private class CountRewriteRule : ICombineRule
		{
			public bool CanApplyAfterProjection(MethodCallExpression node)
			{
				return true;
			}

			public bool CanApplyAfterRowLimit(MethodCallExpression node)
			{
				return true;
			}

			public void Apply(RuleContext context, MethodCallExpression node)
			{

			}

			public Expression Get(RuleContext context, MethodCallExpression node)
			{
				return SpQueryable.MakeCount(node.Method.GetGenericArguments()[0], context.ListItemsProvider, context.Query);
			}
		}

		#endregion
	}

}