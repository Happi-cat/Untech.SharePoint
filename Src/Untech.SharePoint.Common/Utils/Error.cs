using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Converters;
using Untech.SharePoint.Data;
using Untech.SharePoint.Extensions;
using Untech.SharePoint.MetaModels;

namespace Untech.SharePoint.Utils
{
	internal static class Error
	{
		internal static KeyNotFoundException KeyNotFound(object key)
		{
			return new KeyNotFoundException($"Key not found '{key}'");
		}

		public static InvalidOperationException MoreThanOneMatch()
		{
			return new InvalidOperationException("More than one match found");
		}

		public static InvalidOperationException NoMatch()
		{
			return new InvalidOperationException("No match found");
		}

		internal static NotSupportedException SubqueryNotSupported(Expression node)
		{
			return new NotSupportedException($"Sub-query '{node}' is not supported");
		}

		internal static DataMappingException CannotMapFieldToSP(MetaField field, Exception inner)
		{
			var msg = $"Cannot map member '{field.MemberName}' of type '{field.MemberType}' to SP field {field.InternalName}.";
			return new DataMappingException(msg, inner);
		}

		internal static DataMappingException CannotMapFieldFromSP(MetaField field, Exception inner)
		{
			var msg = $"Cannot map member '{field.MemberName}' of type '{field.MemberType}' from SP field {field.InternalName}.";
			return new DataMappingException(msg, inner);
		}

		internal static InvalidOperationException OperationNotAllowedForExternalList()
		{
			return new InvalidOperationException("This operation cannot be used with external list");
		}

		internal static InvalidOperationException OperationRequireIdField()
		{
			return new InvalidOperationException("This operation require ID field");
		}

		internal static FieldConverterException CannotConvertFromSpValue(Type converterType, object spValue, Exception inner)
		{
			var msg = $"SP value '{spValue}' cannot be converted by '{converterType}' field converter";
			return new FieldConverterException(msg, inner);
		}

		internal static FieldConverterException CannotConvertToSpValue(Type converterType, object value, Exception inner)
		{
			var msg = $"Field converter '{converterType}' cannot convert value '{value}' to SP value";
			return new FieldConverterException(msg, inner);
		}

		internal static FieldConverterException CannotConvertToCamlValue(Type converterType, object value, Exception inner)
		{
			var msg = $"Field converter '{converterType}' cannot convert value '{value}' to CAML value";
			return new FieldConverterException(msg, inner);
		}

		internal static NotSupportedException SubqueryNotSupportedAfterProjection(MethodCallExpression node)
		{
			var msg =
				$"Method '.{node.Method.Name}({GetArgs(node.Arguments)})' cannot be applied after any projection method like '.Select'";

			throw new NotSupportedException(msg);
		}

		internal static NotSupportedException SubqueryNotSupportedAfterRowLimit(MethodCallExpression node)
		{
			var msg = $"Method '.{node.Method.Name}({GetArgs(node.Arguments)})' cannot be applied after any row limit method like '.Take'";

			throw new NotSupportedException(msg);
		}

		internal static FieldConverterException ConverterNotFound(string typeAsString)
		{
			return new FieldConverterException($"Cannot find converter '{typeAsString}' in Config");
		}

		internal static FieldConverterException ConverterNotFound(Type converterType)
		{
			return new FieldConverterException($"Cannot find converter '{converterType}' in Config");
		}

		private static string GetArgs(IEnumerable<Expression> arguments)
		{
			var args = arguments.Skip(1).JoinToString(", ");

			return string.IsNullOrEmpty(args) ? "source" : "source, " + args;
		}
	}
}