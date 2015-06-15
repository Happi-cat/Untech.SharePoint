using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Number")]
	internal class NumberFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldNumber>(field, "This Field Converter doesn't support that SPField type");

			return (double?)value;
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldNumber>(field, "This Field Converter doesn't support that SPField type");

			return value;
		}
	}
}
