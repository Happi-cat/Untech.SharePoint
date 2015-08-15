using System.Linq.Expressions;

namespace Untech.SharePoint.Client.Extensions
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

		public static LambdaExpression GetLambda(this Expression node)
		{
			node = node.StripQuotes();
			if (node.NodeType == ExpressionType.Constant)
			{
				return ((ConstantExpression)node).Value as LambdaExpression;
			}
			return node as LambdaExpression;
		}
	}
}
