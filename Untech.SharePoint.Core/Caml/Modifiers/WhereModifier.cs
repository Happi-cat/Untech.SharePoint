using System;
using System.Linq.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	internal class WhereModifier : ExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.Name == "Where")
			{
				var innerMethodCall = node.Arguments[0] as MethodCallExpression;
				if (innerMethodCall != null && innerMethodCall.Method.Name == "Where")
				{
					var currentLambda = (LambdaExpression)node.Arguments[1].StripQuotes();
					var innerLambda = (LambdaExpression)innerMethodCall.Arguments[1].StripQuotes();

					if (currentLambda.Type != innerLambda.Type)
					{
						throw new NotSupportedException("Where methods have predicates with mismatch return type or arguments list");
					}

					var newCondition = Expression.AndAlso(innerLambda.Body, currentLambda.Body);
					var newLambda = Expression.Lambda(newCondition, currentLambda.Parameters);

					return Visit(node.Update(null, new[] { innerMethodCall.Arguments[0], newLambda }));
				}
			}
			return base.VisitMethodCall(node);
		}
	}
}