using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate.NegateRules
{
	internal class BoolConstNegateRule : INegateRule
	{
		public bool CanNegate(Expression node)
		{
			return node.NodeType == ExpressionType.Constant;
		}

		public Expression Negate(Expression node)
		{
			var constNode = (ConstantExpression)node;

			return constNode.Type == typeof(bool) ? Expression.Constant(!(bool)constNode.Value) : constNode;
		}
	}
}