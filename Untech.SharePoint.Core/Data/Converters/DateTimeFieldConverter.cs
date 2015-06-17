using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("DateTime")]
	internal class DateTimeFieldConverter: IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Field = field;
			PropertyType = propertyType;
		}

	    public object FromSpValue(object value)
		{
            Guard.ThrowIfNot<SPFieldDateTime>(Field, "This Field Converter doesn't support that SPField type");

			if (PropertyType.IsNullableType())
				return (DateTime?)value;

			return (DateTime?)value ?? new DateTime(1900, 1, 1);
		}

        public object ToSpValue(object value)
		{
            Guard.ThrowIfNot<SPFieldDateTime>(Field, "This Field Converter doesn't support that SPField type");

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
