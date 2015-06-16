using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("DateTime")]
	internal class DateTimeFieldConverter: IFieldConverter
	{
	    public object FromSpValue(object value, SPField field, Type propertyType)
		{
            Guard.ThrowIfNot<SPFieldDateTime>(field, "This Field Converter doesn't support that SPField type");

			if (propertyType.IsNullableType())
				return (DateTime?)value;

			return (DateTime?)value ?? new DateTime(1900, 1, 1);
		}

        public object ToSpValue(object value, SPField field, Type propertyType)
		{
            Guard.ThrowIfNot<SPFieldDateTime>(field, "This Field Converter doesn't support that SPField type");

	        if (value == null)
	        {
		        return null;
	        }
	        var dateValue = (DateTime) value;
	        if (dateValue <= new DateTime(1900, 1, 1))
	        {
		        return null;
	        }

			return dateValue;
		}
	}
}
