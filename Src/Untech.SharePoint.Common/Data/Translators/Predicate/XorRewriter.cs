using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class XorRewriter : ExpressionVisitor
	{
		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.Method != null || node.NodeType != ExpressionType.ExclusiveOr)
			{
				return base.VisitBinary(node);
			}

			var left = Expression.AndAlso(node.Left, Expression.Not(node.Right));
			var right = Expression.AndAlso(Expression.Not(node.Left), node.Right);

			return Visit(Expression.OrElse(left, right));
		}
	}
}