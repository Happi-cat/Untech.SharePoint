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
	public class CamlExpressionTreeProcessor : ExpressionVisitor
	{
		public interface IRewriterRule
		{
			bool CanApply(MethodCallExpression node);

			Expression Apply(MethodCallExpression node);
		}

		public CamlExpressionTreeProcessor()
		{
			Rules = new Dictionary<MethodInfo, IRewriterRule>
			{
				{OpUtils.QWhere, new WhereRewriterRule()},
				{OpUtils.QAny, new AnyRewriterRule()},
				{OpUtils.QAnyP, new AnyRewriterRule()},
				{OpUtils.QAll, new AllRewriterRule()},

				{OpUtils.QOrderBy, new OrderByRewriterRule {Ascending = true}},
				{OpUtils.QOrderByDescending, new OrderByRewriterRule()},
				{OpUtils.QThenBy, new OrderByRewriterRule {Ascending = true}},
				{OpUtils.QThenrByDescending, new OrderByRewriterRule()},

				{OpUtils.QTake, new TakeRewriterRule()},
				{OpUtils.QSkip, new SkipRewriterRule()},

				{OpUtils.QSingle, new FirstRewriterRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
				{OpUtils.QSingleOrDefault, new FirstRewriterRule {ThrowIfMultiple = true}},

				{OpUtils.QFirst, new FirstRewriterRule {ThrowIfNothing = true}},
				{OpUtils.QFirstOrDefault, new FirstRewriterRule()},

				{OpUtils.QLast, new LastRewriteRule {ThrowIfNothing = true}},
				{OpUtils.QLastOrDefault, new LastRewriteRule()},

				{OpUtils.QElementAt, new ElementAtRewriteRule {ThrowIfNothing = true}},
				{OpUtils.QElementAtOrDefault, new ElementAtRewriteRule()},

				{OpUtils.QReverse, new ReverseRewriteRule()},

				{OpUtils.QCount, new CountRewriteRule()}
			};
		}

		protected IReadOnlyDictionary<MethodInfo, IRewriterRule> Rules { get; set; }

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType != typeof(Queryable))
			{
				return node;
			}

			var sourceArg = Visit(node.Arguments[0]);
			var args = new[] { sourceArg }.Concat(node.Arguments.Skip(1)).ToList();

			var newNode = node.Update(null, args);

			return ApplyRuleToMethodCall(newNode);
		}

		private Expression ApplyRuleToMethodCall(MethodCallExpression node)
		{
			var method = node.Method;
			if (method.IsGenericMethod)
			{
				method = method.GetGenericMethodDefinition();
			}
			if (!Rules.ContainsKey(method))
			{
				return node;
			}

			var rule = Rules[method];
			return rule.CanApply(node) ? rule.Apply(node) : node;
		}

		protected class BaseRewriterRule
		{
			protected MethodCallExpression FindSource(MethodCallExpression node)
			{
				var asQueryableCall = node.Arguments[0].StripQuotes() as MethodCallExpression;
				if (asQueryableCall == null)
				{
					return null;
				}
				return asQueryableCall.Arguments[0].StripQuotes() as MethodCallExpression;
			}

			protected ISpItemsProvider GetItemsProvider(MethodCallExpression source)
			{
				var constNode = source.Arguments[0].StripQuotes() as ConstantExpression;
				return constNode != null ? (ISpItemsProvider)constNode.Value : null;
			}

			protected QueryModel GetQueryModel(MethodCallExpression source)
			{
				var constNode = source.Arguments[1].StripQuotes() as ConstantExpression;
				return constNode != null ? (QueryModel)constNode.Value : null;
			}
		}

		protected class WhereRewriterRule : BaseRewriterRule, IRewriterRule
		{
			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				var predicate = node.Arguments[1];

				model.MergeWheres(new CamlPredicateProcessor().Process(predicate));

				return SpQueryable.MakeAsQueryable(node.Method.GetGenericArguments()[0], SpQueryable.MakeGetSpListItems(node.Method.GetGenericArguments()[0], provider, model));
			}
		}

		protected class AnyRewriterRule : BaseRewriterRule, IRewriterRule
		{
			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				if (node.Arguments.Count == 2)
				{
					var predicate = node.Arguments[1];


					model.MergeWheres(new CamlPredicateProcessor().Process(predicate));
				}

				return SpQueryable.MakeAnySpListItems(node.Method.GetGenericArguments()[0], provider, model);
			}
		}

		protected class AllRewriterRule : BaseRewriterRule, IRewriterRule
		{
			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				var predicate = node.Arguments[1];

				model.MergeWheres(new CamlPredicateProcessor().Process(predicate).Negate());

				return SpQueryable.MakeAnySpListItems(node.Method.GetGenericArguments()[0], provider, model);
			}
		}

		protected class OrderByRewriterRule : BaseRewriterRule, IRewriterRule
		{
			public bool Ascending { get; set; }

			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				var memberNode = (MemberExpression)node.Arguments[1].GetLambda().Body;

				model.MergeOrderBys(new OrderByModel(new FieldRefModel(memberNode.Member), Ascending));

				return SpQueryable.MakeAsQueryable(node.Method.GetGenericArguments()[0], SpQueryable.MakeGetSpListItems(node.Method.GetGenericArguments()[0], provider, model));
			}
		}

		protected class TakeRewriterRule : BaseRewriterRule, IRewriterRule
		{
			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				model.RowLimit = (uint)((ConstantExpression) node.Arguments[1].StripQuotes()).Value;

				return SpQueryable.MakeTakeSpListItems(node.Method.GetGenericArguments()[0], provider, model);
			}
		}

		protected class SkipRewriterRule : BaseRewriterRule, IRewriterRule
		{
			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				var count = (int)((ConstantExpression)node.Arguments[1].StripQuotes()).Value;

				return SpQueryable.MakeSkipSpListItems(node.Method.GetGenericArguments()[0], provider, model, count);
			}
		}

		protected class FirstRewriterRule : BaseRewriterRule, IRewriterRule
		{
			public bool ThrowIfNothing { get; set; }

			public bool ThrowIfMultiple { get; set; }

			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				return SpQueryable.MakeFirstSpListItems(node.Method.GetGenericArguments()[0], provider, model, ThrowIfNothing, ThrowIfMultiple);
			}
		}

		protected class LastRewriteRule : BaseRewriterRule, IRewriterRule
		{
			public bool ThrowIfNothing { get; set; }

			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				model.ReverseOrder();

				return SpQueryable.MakeFirstSpListItems(node.Method.GetGenericArguments()[0], provider, model, ThrowIfNothing, false);
			}
		}

		protected class ElementAtRewriteRule : BaseRewriterRule, IRewriterRule
		{
			public bool ThrowIfNothing { get; set; }

			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				var count = (int)((ConstantExpression)node.Arguments[1].StripQuotes()).Value;

				return SpQueryable.MakeElementAtSpListItems(node.Method.GetGenericArguments()[0], provider, model, count, ThrowIfNothing);
			}
		}

		protected class ReverseRewriteRule : BaseRewriterRule, IRewriterRule
		{
			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				model.ReverseOrder();

				return SpQueryable.MakeGetSpListItems(node.Method.GetGenericArguments()[0], provider, model);
			}
		}

		protected class CountRewriteRule : BaseRewriterRule, IRewriterRule
		{
			public bool CanApply(MethodCallExpression node)
			{
				var source = FindSource(node);
				return source != null && OpUtils.IsOperator(source.Method, OpUtils.SpqGetItems);
			}

			public Expression Apply(MethodCallExpression node)
			{
				var source = FindSource(node);
				var provider = GetItemsProvider(source);
				var model = GetQueryModel(source);

				return SpQueryable.MakeCountSpListItems(node.Method.GetGenericArguments()[0], provider, model);
			}
		}
	}
}