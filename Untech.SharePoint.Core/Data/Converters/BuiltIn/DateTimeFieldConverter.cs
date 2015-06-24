using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("DateTime")]
	public class DateTimeFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(DateTime))
				throw new ArgumentException("SPField with DateTime value type only supported");

			if (!propertyType.In(new[] { typeof(DateTime), typeof(DateTime?) }))
				throw new ArgumentException("This converter can be used only with DateTime or Nullable<DateTime> property types");

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
