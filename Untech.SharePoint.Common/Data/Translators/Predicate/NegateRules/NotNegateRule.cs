using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate.NegateRules
{
	internal class NotNegateRule : INegateRule
	{
		public bool CanNegate(Expression node)
		{
			return node.NodeType == ExpressionType.Not;
		}

		public Expression Negate(Expression node)
		{
			var notNode = (UnaryExpression)node;

			return notNode.Operand;
		}
	}
}