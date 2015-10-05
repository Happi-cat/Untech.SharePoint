using System.Collections.Generic;
using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate.NegateRules
{
	internal class ComparisonNegateRule : INegateRule
	{

		private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> NegateMap = new Dictionary
			<ExpressionType, ExpressionType>
		{
			{ExpressionType.Equal, ExpressionType.NotEqual},
			{ExpressionType.NotEqual, ExpressionType.Equal},
			{ExpressionType.LessThan, ExpressionType.GreaterThanOrEqual},
			{ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan},
			{ExpressionType.GreaterThan, ExpressionType.LessThanOrEqual},
			{ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan}
		};

		public bool CanNegate(Expression node)
		{
			return NegateMap.ContainsKey(node.NodeType);
		}

		public Expression Negate(Expression node)
		{
			var binaryNode = (BinaryExpression)node;

			return Expression.MakeBinary(NegateMap[node.NodeType], binaryNode.Left, binaryNode.Right,
				binaryNode.IsLiftedToNull, binaryNode.Method, binaryNode.Conversion);
		}
	}
}