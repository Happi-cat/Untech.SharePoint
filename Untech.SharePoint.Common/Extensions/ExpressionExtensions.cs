using System;
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

		public static LambdaExpression GetLambda(this Expression node)
		{
			node = node.StripQuotes();
			if (node.NodeType == ExpressionType.Constant)
			{
				return ((ConstantExpression)node).Value as LambdaExpression;
			}
			return node as LambdaExpression;
		}

		public static bool IsConstant(this Expression node, object value)
		{
			var constNode = node.StripQuotes() as ConstantExpression;
			return constNode != null && Equals(value, constNode.Value);
		}
	}
}
