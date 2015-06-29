using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	internal class Evaluator : ExpressionVisitor
	{
		protected override Expression VisitMember(MemberExpression node)
		{
			var innerExpression = node;
			while (innerExpression.Expression.NodeType == ExpressionType.MemberAccess)
			{
				innerExpression = (MemberExpression)innerExpression.Expression;
			}
			if (innerExpression.Expression.NodeType != ExpressionType.Constant)
			{
				return base.VisitMember(node);
			}
			var func = Expression.Lambda(node).Compile();
			return Visit(Expression.Constant(func.DynamicInvoke(null), node.Type));
		}
	}
}