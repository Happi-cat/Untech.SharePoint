using System.Linq.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Finders
{
	internal class ProjectorFinder : ExpressionVisitor
	{
		public Expression Source { get; private set; }
		public LambdaExpression Projector { get; private set; }

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			Visit(node.Arguments[0]);

			if (node.Method.Name == "Select")
			{
				Source = node.Arguments[0];
				Projector = node.Arguments[1].StripQuotes() as LambdaExpression;
			}

			return node;
		}
	}
}