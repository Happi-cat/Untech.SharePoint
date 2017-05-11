using System.Linq.Expressions;

namespace Untech.SharePoint.Data.Translators.Predicate
{
	internal class StringIsNullOrEmptyRewriter : ExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method != MethodUtils.StrIsNullOrEmpty)
			{
				return base.VisitMethodCall(node);
			}

			return Expression.OrElse(Expression.Equal(node.Arguments[0], Expression.Constant(null)),
				Expression.Equal(node.Arguments[0], Expression.Constant("")));
		}
	}
}