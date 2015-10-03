using System.Collections.Generic;
using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.NegateRules
{
	internal class LogicalJoinNegateRule : INegateRule
	{
		private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> NegateMap = new Dictionary
			<ExpressionType, ExpressionType>
		{
			{ExpressionType.And, ExpressionType.Or},
			{ExpressionType.AndAlso, ExpressionType.OrElse},
			{ExpressionType.Or, ExpressionType.And},
			{ExpressionType.OrElse, ExpressionType.AndAlso}
		};

		public bool CanNegate(Expression node)
		{
			return NegateMap.ContainsKey(node.NodeType);
		}

		public Expression Negate(Expression node)
		{
			var binaryNode = (BinaryExpression)node;

			return Expression.MakeBinary(NegateMap[node.NodeType],
				Expression.Not(binaryNode.Left), Expression.Not(binaryNode.Right),
				binaryNode.IsLiftedToNull, binaryNode.Method, binaryNode.Conversion);
		}
	}
}