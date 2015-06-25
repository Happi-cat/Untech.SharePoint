using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("Text")]
	[SPFieldConverter("Note")]
	[SPFieldConverter("Choice")]
	internal class TextFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.NotNull(field, "field");
			Guard.NotNull(propertyType, "propertyType");

			Guard.TypeIs<string>(field.FieldValueType, "field.FieldValueType");

			Guard.TypeIs<string>(propertyType, "propertType");

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
