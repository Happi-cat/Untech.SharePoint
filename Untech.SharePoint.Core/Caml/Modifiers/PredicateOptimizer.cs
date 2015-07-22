using System.Collections.Generic;
using System.Linq.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	internal class PredicateOptimizer : ExpressionVisitor
	{
		private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> BinaryInverseMap = new Dictionary<ExpressionType, ExpressionType>
		{
			{ExpressionType.AndAlso, ExpressionType.OrElse},
			{ExpressionType.OrElse, ExpressionType.AndAlso}
		};

		private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> CompareInverseMap = new Dictionary<ExpressionType, ExpressionType>
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

			if (BinaryInverseMap.ContainsKey(notOperand.NodeType))
			{
				return Visit(InvertBinaryAndOr(notOperand));
			}
			if (CompareInverseMap.ContainsKey(notOperand.NodeType))
			{
				return Visit(InvertBinaryCompare(notOperand));
			}
			if (notOperand.NodeType == ExpressionType.Not)
			{
				return Visit(((UnaryExpression)notOperand).Operand);
			}
			return base.VisitUnary(node);
		}

		private static BinaryExpression InvertBinaryCompare(Expression node)
		{
			var binaryNode = (BinaryExpression) node;

			return Expression.MakeBinary(CompareInverseMap[node.NodeType], binaryNode.Left, binaryNode.Right,
				binaryNode.IsLiftedToNull, binaryNode.Method, binaryNode.Conversion);
		}

		private static BinaryExpression InvertBinaryAndOr(Expression node)
		{
			var binaryNode = (BinaryExpression) node;

			return Expression.MakeBinary(BinaryInverseMap[node.NodeType],
				Expression.Not(binaryNode.Left), Expression.Not(binaryNode.Right),
				binaryNode.IsLiftedToNull, binaryNode.Method, binaryNode.Conversion);
		}
	}
}