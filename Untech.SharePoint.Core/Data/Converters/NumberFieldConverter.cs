using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Number")]
	internal class NumberFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldNumber>(field, "This Field Converter doesn't support that SPField type");

			if (propertyType.IsNullableType())
				return (double?)value;

			return (double?) value ?? 0;
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldNumber>(field, "This Field Converter doesn't support that SPField type");

			return (double?)value;
		}
	}
}
