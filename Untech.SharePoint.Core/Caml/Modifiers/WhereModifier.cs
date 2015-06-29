using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	public class WhereModifier : ExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.Name == "Where")
			{
				var innerMethodCall = node.Arguments[0] as MethodCallExpression;
				if (innerMethodCall != null && innerMethodCall.Method.Name == "Where")
				{
					var currentLambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
					var innerLambda = (LambdaExpression)StripQuotes(innerMethodCall.Arguments[1]);

					if (currentLambda.ReturnType != innerLambda.ReturnType ||
						!ParametersEqual(currentLambda.Parameters, innerLambda.Parameters))
					{
						throw new Exception("Unsupported lambda predicate in Where method call");
					}

					var newCondition = Expression.AndAlso(currentLambda.Body, innerLambda.Body);
					var newLambda = Expression.Lambda(newCondition, currentLambda.Parameters);

					return Visit(node.Update(null, new[] { innerMethodCall.Arguments[0], newLambda }));
				}
			}
			return base.VisitMethodCall(node);
		}

		private static Expression StripQuotes(Expression node)
		{
			while (node.NodeType == ExpressionType.Quote)
			{
				node = ((UnaryExpression)node).Operand;
			}
			return node;
		}

		private static bool ParametersEqual(IEnumerable<ParameterExpression> left, IEnumerable<ParameterExpression> right)
		{
			var leftParams = left.Select(n => new Tuple<string, Type>(n.Name, n.Type));
			var rightParams = right.Select(n => new Tuple<string, Type>(n.Name, n.Type));
			return leftParams.SequenceEqual(rightParams);
		}
	}
}