using System.Linq.Expressions;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class RedundantConditionRemover : ExpressionVisitor
	{
		protected override Expression VisitBinary(BinaryExpression node)
		{
			var resultNode = base.VisitBinary(node);

			if (resultNode.NodeType == ExpressionType.Or || resultNode.NodeType == ExpressionType.OrElse)
			{
				return RemoveReduntantOrCondition((BinaryExpression)resultNode);
			}
			if (resultNode.NodeType == ExpressionType.And || resultNode.NodeType == ExpressionType.AndAlso)
			{
				return RemoveReduntantAndCondition((BinaryExpression)resultNode);
			}
			return resultNode;
		}

		private Expression RemoveReduntantOrCondition(BinaryExpression node)
		{
			if (node.Left.IsConstant(true) || node.Right.IsConstant(true))
			{
				return Expression.Constant(true);
			}
			if (node.Left.IsConstant(false))
			{
				return node.Right;
			}
			if (node.Right.IsConstant(false))
			{
				return node.Left;
			}
			return node;
		}

		private Expression RemoveReduntantAndCondition(BinaryExpression node)
		{
			if (node.Left.IsConstant(false) || node.Right.IsConstant(false))
			{
				return Expression.Constant(false);
			}
			if (node.Left.IsConstant(true))
			{
				return node.Right;
			}
			if (node.Right.IsConstant(true))
			{
				return node.Left;
			}
			return node;
		}
	}
}