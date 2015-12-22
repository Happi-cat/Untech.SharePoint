using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Utils
{
	internal static class Error
	{
		internal static Exception KeyNotFound(object key)
		{
			return new KeyNotFoundException(string.Format("Key not found '{0}'", key));
		}

		public static Exception MoreThanOneMatch()
		{
			return new InvalidOperationException("More than one match found");
		}

		public static Exception NoMatch()
		{
			return new InvalidOperationException("No match found");
		}

		internal static Exception SubqueryNotSupported(Expression node)
		{
			return new NotSupportedException(string.Format("Subquery '{0}' is not supported", node));
		}

		internal static Exception CannotMapFieldToSP(MetaField field, Exception inner)
		{
			var msg = string.Format("Cannot map member '{0}' of type '{1}' to SP field {2}.",
				field.Member, field.MemberType, field.InternalName);
			return new DataMappingException(msg, inner);
		}

		internal static Exception CannotMapFieldFromSP(MetaField field, Exception inner)
		{
			var msg = string.Format("Cannot map member '{0}' of type '{1}' from SP field {2}.",
				field.Member, field.MemberType, field.InternalName);
			return new DataMappingException(msg, inner);
		}

		internal static Exception OperationNotAllowedForExternalList()
		{
			return new InvalidOperationException("This operation cannot be used with external list");
		}

		internal static Exception OperationRequireIdField()
		{
			return new InvalidOperationException("This operation require ID field");
		}

		internal static Exception CannotConvertFromSpValue(Type converterType, object spValue, Exception inner)
		{
			var msg = string.Format("SP value '{0}' cannot be converted by '{1}' field converter", spValue, converterType);
			return new FieldConverterException(msg, inner);
		}

		internal static Exception CannotConvertToSpValue(Type converterType, object value, Exception inner)
		{
			var msg = string.Format("Field converter '{0}' cannot convert value '{1}' to SP value", converterType, value);
			return new FieldConverterException(msg, inner);
		}

		internal static Exception CannotConvertToCamlValue(Type converterType, object value, Exception inner)
		{
			var msg = string.Format("Field converter '{0}' cannot convert value '{1}' to CAML value", converterType, value);
			return new FieldConverterException(msg, inner);
		}

		internal static Exception SubqueryNotSupportedAfterProjection(MethodCallExpression node)
		{
			var msg = string.Format("Method '.{0}({1})' cannot be applied after any projection method like '.Select'", 
				node.Method.Name,
				GetArgs(node.Arguments));

			throw new NotSupportedException(msg);
		}

		internal static Exception SubqueryNotSupportedAfterRowLimit(MethodCallExpression node)
		{
			var msg = string.Format("Method '.{0}({1})' cannot be applied after any row limit method like '.Take'",
				node.Method.Name, 
				GetArgs(node.Arguments));

			throw new NotSupportedException(msg);
		}

		internal static Exception ConverterNotFound(string typeAsString)
		{
			return new FieldConverterException(string.Format("Cannot find converter '{0}' in Config",typeAsString));
		}

		internal static Exception ConverterNotFound(Type converterType)
		{
			return new FieldConverterException(string.Format("Cannot find converter '{0}' in Config", converterType));
		}

		private static string GetArgs(IEnumerable<Expression> arguments)
		{
			var args = arguments.Skip(1).JoinToString(", ");

			return string.IsNullOrEmpty(args) ? "source" : "source, " + args;
		}
	}
}