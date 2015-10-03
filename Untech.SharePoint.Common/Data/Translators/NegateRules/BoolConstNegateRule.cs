using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.NegateRules
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

			if (constNode.Value is bool)
			{
				return Expression.Constant(!(bool)constNode.Value);
			}

			return constNode;
		}
	}
}