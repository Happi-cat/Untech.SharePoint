using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	public class AnyAllModifer : ExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			switch (node.Method.Name)
			{
				case "All":
				case "Any":
					if (node.Arguments.Count == 2)
					{
						var predicate = (LambdaExpression)node.Arguments[1].StripQuotes();
						node = node.Update(null, new[]
						{
							Expression.Call(typeof (Queryable), "Where", node.Method.GetGenericArguments(), node.Arguments[0], predicate),
						});

					}
					break;
			}

			Visit(node.Arguments[0]);
			return node;
		}

	}
}