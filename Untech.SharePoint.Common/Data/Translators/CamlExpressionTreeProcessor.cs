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

		private static readonly IReadOnlyDictionary<MethodInfo, IRewriterRule> Rules = new Dictionary
			<MethodInfo, IRewriterRule>
		{
			{OpUtils.QWhere, new WhereRewriterRule()},
			{OpUtils.QAny, new AnyRewriterRule()},
			{OpUtils.QAnyP, new AnyRewriterRule()},
			{OpUtils.QAll, new AllRewriterRule()},
			{OpUtils.QOrderBy, new OrderByRewriterRule(OrderByRewriterRuleType.Asc)},
			{OpUtils.QOrderByDescending, new OrderByRewriterRule(OrderByRewriterRuleType.Desc)},
			{OpUtils.QThenBy, new OrderByRewriterRule(OrderByRewriterRuleType.Asc)},
			{OpUtils.QThenrByDescending, new OrderByRewriterRule(OrderByRewriterRuleType.Desc)},

		};

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

		protected enum OrderByRewriterRuleType
		{
			Asc,
			Desc
		}

		protected class OrderByRewriterRule : BaseRewriterRule, IRewriterRule
		{
			public OrderByRewriterRule(OrderByRewriterRuleType type)
			{
				
			}

			public bool CanApply(MethodCallExpression node)
			{
				throw new System.NotImplementedException();
			}

			public Expression Apply(MethodCallExpression node)
			{
				throw new System.NotImplementedException();
			}
		}


	}
}