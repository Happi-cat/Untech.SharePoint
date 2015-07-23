using System;
using System.Collections.Generic;
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
				case "First":
				case "FirstOrDefault":
				case "Single":
				case "SingleOrDefault":
					if (node.Arguments.Count == 2)
					{
						var predicate = ConvertToWherePredicate((LambdaExpression)node.Arguments[1].StripQuotes(), node.Method.Name == "All");

						var whereExpression = Expression.Call(typeof (Queryable), "Where", node.Method.GetGenericArguments(),
							node.Arguments[0], predicate);

						var methodName = node.Method.Name;
						if (methodName == "All")
						{
							methodName = "Any";
						}

						node = Expression.Call(node.Type, methodName, node.Method.GetGenericArguments(), whereExpression);
					}
					break;
			}

			Visit(node.Arguments[0]);
			return node;
		}

		private static LambdaExpression ConvertToWherePredicate(LambdaExpression predicate, bool isAll = false)
		{
			return isAll ? Expression.Lambda(Expression.Not(predicate.Body), predicate.Parameters) : predicate;
		}
	}
}