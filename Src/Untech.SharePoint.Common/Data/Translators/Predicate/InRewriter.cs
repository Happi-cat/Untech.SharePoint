using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class InRewriter : ExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (IsValidObjectInCall(node))
			{
				return RewriteIn((MemberExpression)node.Arguments[0], (ConstantExpression)node.Arguments[1]);
			}
			if (IsValidEnumerableContainsCall(node))
			{
				return RewriteIn((MemberExpression)node.Arguments[1], (ConstantExpression)node.Arguments[0]);
			}
			if (IsValidListContainsCall(node))
			{
				return RewriteIn((MemberExpression)node.Arguments[0], (ConstantExpression)node.Object);
			}
			return base.VisitMethodCall(node);
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			var innerOperand = node.Operand.StripQuotes() as MethodCallExpression;
			if (node.NodeType == ExpressionType.Not && innerOperand != null)
			{
				if (IsValidObjectInCall(innerOperand))
				{
					return RewriteNotIn((MemberExpression)innerOperand.Arguments[0], (ConstantExpression)innerOperand.Arguments[1]);
				}
				if (IsValidEnumerableContainsCall(innerOperand))
				{
					return RewriteNotIn((MemberExpression)innerOperand.Arguments[1], (ConstantExpression)innerOperand.Arguments[0]);
				}
				if (IsValidListContainsCall(innerOperand))
				{
					return RewriteNotIn((MemberExpression)innerOperand.Arguments[0], (ConstantExpression)innerOperand.Object);
				}
			}

			return base.VisitUnary(node);
		}

		#region [Private Methods]

		private static bool IsValidObjectInCall([NotNull]MethodCallExpression node)
		{
			return MethodUtils.IsOperator(node.Method, MethodUtils.ObjIn)
				   && node.Arguments[0].NodeType == ExpressionType.MemberAccess
				   && node.Arguments[1].NodeType == ExpressionType.Constant;
		}

		private static bool IsValidEnumerableContainsCall([NotNull]MethodCallExpression node)
		{
			return MethodUtils.IsOperator(node.Method, MethodUtils.EContains)
				   && node.Arguments[0].NodeType == ExpressionType.Constant
				   && node.Arguments[1].NodeType == ExpressionType.MemberAccess;
		}

		private static bool IsValidListContainsCall([NotNull]MethodCallExpression node)
		{
			return MethodUtils.IsOperator(node.Method, MethodUtils.ListContains)
				   && node.Object != null
				   && node.Object.NodeType == ExpressionType.Constant
				   && node.Arguments[0].NodeType == ExpressionType.MemberAccess;
		}

		private Expression RewriteIn(MemberExpression memberNode, ConstantExpression arrayNode)
		{
			var array = (IEnumerable)arrayNode.Value ?? new object[0];

			return array.Cast<object>().Aggregate<object, Expression>(null, (whereNode, value) =>
			{
				var eqNode = Expression.Equal(memberNode, Expression.Constant(value));

				return whereNode == null ? eqNode : Expression.OrElse(whereNode, eqNode);
			});
		}

		private Expression RewriteNotIn(MemberExpression memberNode, ConstantExpression arrayNode)
		{
			var array = (IEnumerable)arrayNode.Value;

			return array.Cast<object>().Aggregate<object, Expression>(null, (whereNode, value) =>
			{
				var notEqualNode = Expression.NotEqual(memberNode, Expression.Constant(value));

				return whereNode == null ? notEqualNode : Expression.AndAlso(whereNode, notEqualNode);
			});
		}

		#endregion
	}
}