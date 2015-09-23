using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("DateTime")]
	internal class DateTimeFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			if (Field.MemberType == typeof(DateTime?))
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

		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}
	}
}
