using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.Data.QueryModels;
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

		private static readonly IReadOnlyDictionary<MethodInfo, IRewriterRule> Rules = new Dictionary<MethodInfo, IRewriterRule>
		{
			{OpUtils.QWhere, new WhereRewriterRule()}
		};

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType != typeof (Queryable))
			{
				return node;
			}

			var sourceArg = Visit(node.Arguments[0]);
			var args = new[] {sourceArg}.Concat(node.Arguments.Skip(1)).ToList();

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
				return node.Arguments[0].StripQuotes() as MethodCallExpression;
			}

			protected ISpItemsProvider GetItemsProvider(MethodCallExpression source)
			{
				var constNode = source.Arguments[0].StripQuotes() as ConstantExpression;
				return constNode != null ? (ISpItemsProvider) constNode.Value : null;
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

				return SpQueryable.MakeGetSpListItems(node.Method.GetGenericArguments()[0], provider, model);
			}
		}
	}
}