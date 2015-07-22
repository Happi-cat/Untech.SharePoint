using System.Linq.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	internal class BoolEqualityModifier : ExpressionVisitor
	{
		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.NodeType != ExpressionType.AndAlso && node.NodeType != ExpressionType.OrElse)
			{
				return base.VisitBinary(node);
			}

			var left = node.Left.StripQuotes();
			var right = node.Right.StripQuotes();

			if (CanMakeEqualTrueFalse(left))
			{
				left = MakeEqualTrueFalse(left);
			}

			if (CanMakeEqualTrueFalse(right))
			{
				right = MakeEqualTrueFalse(right);
			}

			return base.VisitBinary(node.Update(left, VisitAndConvert(node.Conversion, "VisitBinary"), right));
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.Name == "Where")
			{
				var lambda = (LambdaExpression)node.Arguments[1].StripQuotes();
				var predicate = lambda.Body.StripQuotes();

				if (CanMakeEqualTrueFalse(predicate))
				{
					var newLambda = Expression.Lambda(MakeEqualTrueFalse(predicate), lambda.Parameters);

					return base.VisitMethodCall(node.Update(null, new[] { node.Arguments[0], newLambda }));
				}
			}

			return base.VisitMethodCall(node);
		}

		#region [Private Methods]

		private static bool CanMakeEqualTrueFalse(Expression node)
		{
			return node.NodeType == ExpressionType.MemberAccess ||
			       node.NodeType == ExpressionType.Not;
		}

		private static Expression MakeEqualTrueFalse(Expression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.MemberAccess:
					return MakeEqualTrueFalse(node, true);
				case ExpressionType.Not:
					var notExpression = (UnaryExpression)node;
					var notOperand = notExpression.Operand.StripQuotes();
					if (notOperand.NodeType == ExpressionType.MemberAccess)
					{
						return MakeEqualTrueFalse(notOperand, false);
					}
					break;
			}
			return node;
		}

		private static Expression MakeEqualTrueFalse(Expression node, bool equalTruth)
		{
			return Expression.Equal(node, Expression.Constant(equalTruth));
		}

		#endregion
	}
}