using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Finders
{
	internal class SpModelContextFinder : ExpressionVisitor
	{
		public ISpModelContext ModelContext { get; set; }

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			Visit(node.Arguments[0]);

			return node;
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			if (typeof (ISpModelContext).IsAssignableFrom(node.Type))
			{
				ModelContext = (ISpModelContext) node.Value;
			}
			
			return node;
		}
	}
}