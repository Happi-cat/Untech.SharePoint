using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Extensions
{
	internal static class ExpressionExtensions
	{
		public static Expression StripQuotes(this Expression node)
		{
			while (node.NodeType == ExpressionType.Quote)
			{
				node = ((UnaryExpression)node).Operand;
			}
			return node;
		}

		public static bool IsConstant(this Expression node, object value)
		{
			var constNode = node.StripQuotes() as ConstantExpression;
			return constNode != null && Equals(value, constNode.Value);
		}
	}
}
