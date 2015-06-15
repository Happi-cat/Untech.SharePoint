using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("DateTime")]
	internal class DateTimeFieldConverter: IFieldConverter
	{
	    public object FromSpValue(object value, SPField field, Type propertyType)
		{
            Guard.ThrowIfNot<SPFieldDateTime>(field, "This Field Converter doesn't support that SPField type");

			return (DateTime?)value;
		}

        public object ToSpValue(object value, SPField field, Type propertyType)
		{
            Guard.ThrowIfNot<SPFieldDateTime>(field, "This Field Converter doesn't support that SPField type");

			return value;
		}
	}
}
