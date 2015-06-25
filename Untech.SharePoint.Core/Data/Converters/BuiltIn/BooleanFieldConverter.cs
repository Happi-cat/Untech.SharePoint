using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("Boolean")]
	internal class BooleanFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.ThrowIfArgumentNull(field, "field");
			Guard.ThrowIfArgumentNull(propertyType, "propertyType");

			Guard.ThrowIfArgumentNotIs<bool>(field.FieldValueType, "field.FieldValueType");

			Guard.ThrowIfArgumentNotIs(propertyType, new[] { typeof(bool), typeof(bool?) }, "propertType");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			return PropertyType == typeof (bool?) ? (bool?) value : ((bool?) value ?? false);
		}

		public object ToSpValue(object value)
		{
			return (bool?)value;
		}
	}
}