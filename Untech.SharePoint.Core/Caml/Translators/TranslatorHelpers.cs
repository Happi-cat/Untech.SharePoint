using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal static class TranslatorHelpers
	{
		public static XElement GetFieldRef(ISpModelContext modelContext, Expression node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			node = node.StripQuotes();

			if (node.NodeType != ExpressionType.MemberAccess)
			{
				throw new NotSupportedException(string.Format("{0} should be a member access", node));
			}

			var memberExpression = (MemberExpression) node;
			var memberName = memberExpression.Member.Name;

			if (memberExpression.Expression.NodeType != ExpressionType.Parameter)
			{
				throw new NotSupportedException(string.Format("{0} not supported", memberExpression));
			}

			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Name, modelContext.GetSpFieldInternalName(memberExpression.Member.DeclaringType, memberName)));
		}


		public static XElement GetValue(ISpModelContext modelContext, MemberExpression memberExpression, Expression node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			node = node.StripQuotes();

			if (node.NodeType != ExpressionType.Constant)
			{
				throw new NotSupportedException(string.Format("{0} should be a constant expression", node));
			}

			var constantExpression = (ConstantExpression)node;

			return new XElement(Tags.Value, 
				new XAttribute(Tags.Type, modelContext.GetSpFieldTypeAsString(memberExpression.Member.DeclaringType, memberExpression.Member.Name)),
				Convert.ToString(modelContext.ConvertToSpValue(memberExpression.Member.DeclaringType, memberExpression.Member.Name, constantExpression.Value)));
		}
	}
}