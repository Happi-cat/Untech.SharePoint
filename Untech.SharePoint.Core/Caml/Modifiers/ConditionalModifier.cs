using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	public class ConditionalModifier : ExpressionVisitor
	{
		protected override Expression VisitBinary(BinaryExpression node)
		{
			var left = StripQuotes(node.Left);
			var right = StripQuotes(node.Right);

			if (node.NodeType == ExpressionType.AndAlso || node.NodeType == ExpressionType.OrElse)
			{
				if (left.NodeType == ExpressionType.MemberAccess ||
					left.NodeType == ExpressionType.Not)
				{
					left = MakeEqualTrueFalse(left);
				}

				if (right.NodeType == ExpressionType.MemberAccess ||
					right.NodeType == ExpressionType.Not)
				{
					right = MakeEqualTrueFalse(right);
				}

				node = node.Update(left, VisitAndConvert(node.Conversion, "VisitBinary"), right);
			}
			else if (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual)
			{
				if (left.NodeType == ExpressionType.Constant)
				{
					node = node.Update(right, VisitAndConvert(node.Conversion, "VisitBinary"), left);
				}
			}

			return base.VisitBinary(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.Name == "Where")
			{
				var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
				var predicate = StripQuotes(lambda.Body);

				if (predicate.NodeType == ExpressionType.MemberAccess ||
					predicate.NodeType == ExpressionType.Not)
				{
					var newLambda = Expression.Lambda(MakeEqualTrueFalse(predicate), lambda.Parameters);

					node = node.Update(null, new[] { node.Arguments[0], newLambda });
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

		private static Expression MakeEqualTrueFalse(Expression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.MemberAccess:
					return MakeEqualTrueFalse((MemberExpression)node, true);
				case ExpressionType.Not:
					var notExp = (UnaryExpression)node;
					var innerExp = StripQuotes(notExp.Operand);
					if (innerExp.NodeType == ExpressionType.MemberAccess)
					{
						return MakeEqualTrueFalse((MemberExpression)innerExp, false);
					}
					break;
			}
			return null;
		}

		private static Expression MakeEqualTrueFalse(MemberExpression node, bool equalTruth)
		{
			return Expression.Equal(node, Expression.Constant(equalTruth));
		}
	}
}