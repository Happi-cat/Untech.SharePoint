using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Number")]
	internal class NumberFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(double))
				throw new ArgumentException("SPField with bool value type only supported");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			if (PropertyType.IsNullableType())
				return (double?)value;

			return (double?) value ?? 0;
		}

		public object ToSpValue(object value)
		{
			return (double?)value;
		}
	}
}
