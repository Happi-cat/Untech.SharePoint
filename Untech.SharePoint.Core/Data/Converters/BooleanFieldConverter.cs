using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Boolean")]
	public class BooleanFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldBoolean>(field, "This Field Converter doesn't support that SPField type");

			if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return (bool?)value;

			return (bool?)value ?? false;
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldBoolean>(field, "This Field Converter doesn't support that SPField type");

			return (bool?)value;
		}
	}
}