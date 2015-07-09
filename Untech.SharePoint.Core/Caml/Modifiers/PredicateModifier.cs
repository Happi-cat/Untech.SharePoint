using System.Collections.Generic;
using System.Linq.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	internal class PredicateModifier : ExpressionVisitor
	{
		private readonly IReadOnlyDictionary<ExpressionType, ExpressionType> _binaryInverseMap = new Dictionary<ExpressionType, ExpressionType>
		{
			{ExpressionType.AndAlso, ExpressionType.OrElse},
			{ExpressionType.OrElse, ExpressionType.AndAlso}
		};

		private readonly IReadOnlyDictionary<ExpressionType, ExpressionType> _compareInverseMap = new Dictionary<ExpressionType, ExpressionType>
		{
			{ExpressionType.Equal, ExpressionType.NotEqual},
			{ExpressionType.LessThan, ExpressionType.GreaterThanOrEqual},
			{ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan},
			{ExpressionType.GreaterThan, ExpressionType.LessThanOrEqual},
			{ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan}
		};

		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.NodeType == ExpressionType.AndAlso || 
				node.NodeType == ExpressionType.OrElse)
			{
				var left = node.Left.StripQuotes();
				var right = node.Right.StripQuotes();
				var predicateUpdated = false;

				if (CanMakeEqualTrueFalse(left))
				{
					left = MakeEqualTrueFalse(left);
					predicateUpdated = true;
				}

				if (CanMakeEqualTrueFalse(right))
				{
					right = MakeEqualTrueFalse(right);
					predicateUpdated = true;
				}

				if (predicateUpdated)
				{
					return base.VisitBinary(node.Update(left, VisitAndConvert(node.Conversion, "VisitBinary"), right));
				}
			}

			return base.VisitBinary(node);
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.NodeType == ExpressionType.Not)
			{
				var innerOperand = node.Operand.StripQuotes();

				if (_binaryInverseMap.ContainsKey(innerOperand.NodeType))
				{
					var binary = (BinaryExpression)innerOperand;

					var newBinary = Expression.MakeBinary(_binaryInverseMap[innerOperand.NodeType], 
						Expression.Not(binary.Left), Expression.Not(binary.Right),
						binary.IsLiftedToNull, binary.Method, binary.Conversion);

					return Visit(newBinary);
				}
				if (_compareInverseMap.ContainsKey(innerOperand.NodeType))
				{
					var binary = (BinaryExpression)innerOperand;

					var newBinary = Expression.MakeBinary(_compareInverseMap[innerOperand.NodeType], binary.Left, binary.Right,
						binary.IsLiftedToNull, binary.Method, binary.Conversion);

					return Visit(newBinary);
				}
			}
			return base.VisitUnary(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.Name == "Where")
			{
				var lambda = (LambdaExpression) node.Arguments[1].StripQuotes();
				var predicate = lambda.Body.StripQuotes();

				if (CanMakeEqualTrueFalse(predicate))
				{
					var newLambda = Expression.Lambda(MakeEqualTrueFalse(predicate), lambda.Parameters);

					return base.VisitMethodCall(node.Update(null, new[] {node.Arguments[0], newLambda}));
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
					return MakeEqualTrueFalse((MemberExpression) node, true);
				case ExpressionType.Not:
					var notExpression = (UnaryExpression) node;
					var innerExpression = notExpression.Operand.StripQuotes();
					if (innerExpression.NodeType == ExpressionType.MemberAccess)
					{
						return MakeEqualTrueFalse((MemberExpression) innerExpression, false);
					}
					break;
			}
			return node;
		}

		private static Expression MakeEqualTrueFalse(MemberExpression node, bool equalTruth)
		{
			return Expression.Equal(node, Expression.Constant(equalTruth));
		}

		#endregion

	}
}