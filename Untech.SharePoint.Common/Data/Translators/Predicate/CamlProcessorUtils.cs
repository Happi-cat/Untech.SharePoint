using System;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal static class CamlProcessorUtils
	{
		internal static FieldRefModel GetFieldRef(Expression node)
		{
			if (node.NodeType == ExpressionType.MemberAccess)
			{
				var memberNode = (MemberExpression)node;
				var objectNode = memberNode.Expression;
				if (objectNode != null && (objectNode.NodeType == ExpressionType.Convert || objectNode.NodeType == ExpressionType.ConvertChecked))
				{
					var convertNode = (UnaryExpression) objectNode;
					objectNode = convertNode.Operand;
				}
				if (objectNode != null && objectNode.NodeType == ExpressionType.Parameter)
				{
					return new FieldRefModel(memberNode.Member);
				}
			}
			throw InvalidQuery(node);
		}

		internal static Exception InvalidQuery(Expression node)
		{
			return new NotSupportedException(node.ToString());
		}
	}
}