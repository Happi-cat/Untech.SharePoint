using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Extensions
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
	}
}
