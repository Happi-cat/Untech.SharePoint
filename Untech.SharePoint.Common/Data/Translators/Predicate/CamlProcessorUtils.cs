using System;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Utils;

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
				if (objectNode != null && objectNode.NodeType.In(new [] { ExpressionType.Convert, ExpressionType.ConvertChecked}))
				{
					var convertNode = (UnaryExpression) objectNode;
					objectNode = convertNode.Operand;
				}
				if (objectNode != null && objectNode.NodeType == ExpressionType.Parameter)
				{
					return new FieldRefModel(memberNode.Member);
				}
			}
			throw Error.SubqueryNotSupported(node);
		}
	}
}