using System.Collections.Generic;
using System.Linq.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	internal class EqualityAdder : ExpressionVisitor
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

	internal class PredicateModifier : ExpressionVisitor
	{
		private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> _binaryInverseMap = new Dictionary<ExpressionType, ExpressionType>
		{
			{ExpressionType.AndAlso, ExpressionType.OrElse},
			{ExpressionType.OrElse, ExpressionType.AndAlso}
		};

		private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> _compareInverseMap = new Dictionary<ExpressionType, ExpressionType>
		{
			{ExpressionType.Equal, ExpressionType.NotEqual},
			{ExpressionType.LessThan, ExpressionType.GreaterThanOrEqual},
			{ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan},
			{ExpressionType.GreaterThan, ExpressionType.LessThanOrEqual},
			{ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan}
		};

		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.NodeType != ExpressionType.Not)
			{
				return base.VisitUnary(node);
			}

			var notOperand = node.Operand.StripQuotes();

			if (_binaryInverseMap.ContainsKey(notOperand.NodeType))
			{
				var binaryNode = (BinaryExpression)notOperand;

				var newBinaryNode = Expression.MakeBinary(_binaryInverseMap[notOperand.NodeType],
					Expression.Not(binaryNode.Left), Expression.Not(binaryNode.Right),
					binaryNode.IsLiftedToNull, binaryNode.Method, binaryNode.Conversion);

				return Visit(newBinaryNode);
			}
			if (_compareInverseMap.ContainsKey(notOperand.NodeType))
			{
				var binaryNode = (BinaryExpression)notOperand;

				var newBinaryNode = Expression.MakeBinary(_compareInverseMap[notOperand.NodeType], binaryNode.Left, binaryNode.Right,
					binaryNode.IsLiftedToNull, binaryNode.Method, binaryNode.Conversion);

				return Visit(newBinaryNode);
			}
			if (notOperand.NodeType == ExpressionType.Not)
			{
				return Visit(((UnaryExpression)notOperand).Operand);
			}
			return base.VisitUnary(node);
		}
	}
}