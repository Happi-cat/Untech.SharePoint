using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SpFieldConverter("Text")]
	[SpFieldConverter("Note")]
	[SpFieldConverter("Choice")]
	internal class TextFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.ThrowIfArgumentNull(field, "field");
			Guard.ThrowIfArgumentNull(propertyType, "propertyType");

			Guard.ThrowIfArgumentNotIs<string>(field.FieldValueType, "field.FieldValueType");

			Guard.ThrowIfArgumentNotIs<string>(propertyType, "propertType");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			return value == null ? null : value.ToString();
		}

		public object ToSpValue(object value)
		{
			return value == null ? null : value.ToString();
		}
	}
}
