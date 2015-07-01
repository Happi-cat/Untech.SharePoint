using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal static class TranslatorHelpers
	{
		public static XElement GetFieldRef(Expression node)
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
				new XAttribute(Tags.Name, memberName));
		}


		public static XElement GetValue(Expression node)
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
				Convert.ToString(constantExpression.Value));
		}
	}
}