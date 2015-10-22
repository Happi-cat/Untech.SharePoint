using System;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Utils
{
	internal static class Error
	{
		internal static Exception MoreThanOneMatch()
		{
			return new InvalidOperationException("More than one match found");
		}

		internal static Exception NoMatch()
		{
			return new InvalidOperationException("No match found");
		}

		internal static Exception SubqueryNotSupported(Expression node)
		{
			return new NotSupportedException(string.Format("Subquery '{0}' is not supported", node));
		}

		internal static Exception CannotMapField(MetaField field)
		{
			return new DataMappingException(field);
		}

		internal static Exception CannotMapField(MetaField field, Exception inner)
		{
			return new DataMappingException(field, inner);
		}

		internal static Exception SubqueryNegateNotSupported(WhereModel whereModel)
		{
			return new NotSupportedException(string.Format("Subquery '{0}' cannot be negated", whereModel));
		}
	}
}