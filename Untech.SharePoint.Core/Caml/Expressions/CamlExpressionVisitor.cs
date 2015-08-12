using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal class CamlExpressionVisitor : ExpressionVisitor
	{
		protected internal virtual Expression VisitTodayExpression(TodayExpression node)
		{
			return node.Update(Visit(node.Offset));
		}

		protected internal virtual Expression VisitNowExpression(NowExpression node)
		{
			return node;
		}

		protected internal virtual Expression VisitFieldRefExpression(FieldRefExpression node)
		{
			return node;
		}

		protected internal virtual Expression VisitOrderingExpression(OrderingExpression node)
		{
			return node;
		}

		protected internal virtual Expression VisitQueryExpression(QueryExpression node)
		{
			return node.Update(Visit(node.Where), 
				Visit(node.OrderBy), 
				Visit(node.GroupBy));
		}

		protected internal virtual Expression VisitViewExpression(ViewExpression node)
		{
			return node.Update(Visit(node.ViewFields),
				Visit(node.Query),
				Visit(node.RowLimit));
		}
	}
}