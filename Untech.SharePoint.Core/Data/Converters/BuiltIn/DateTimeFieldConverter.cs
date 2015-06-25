using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("DateTime")]
	internal class DateTimeFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.ThrowIfArgumentNull(field, "field");
			Guard.ThrowIfArgumentNull(propertyType, "propertyType");

			Guard.ThrowIfArgumentNotIs<DateTime>(field.FieldValueType, "field.FieldValueType");

			Guard.ThrowIfArgumentNotIs(propertyType, new[] { typeof(DateTime), typeof(DateTime?) }, "propertType");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			if (PropertyType == typeof(DateTime?))
				return (DateTime?)value;

			return (DateTime?)value ?? new DateTime(1900, 1, 1);
		}

		public object ToSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			var dateValue = (DateTime)value;
			if (dateValue <= new DateTime(1900, 1, 1))
			{
				return null;
			}

			return dateValue;
		}
	}
}
