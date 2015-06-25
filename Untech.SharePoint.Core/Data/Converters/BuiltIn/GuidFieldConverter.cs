using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("Guid")]
	internal class GuidFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.ThrowIfArgumentNull(field, "field");
			Guard.ThrowIfArgumentNull(propertyType, "propertyType");

			Guard.ThrowIfArgumentNotIs<Guid>(field.FieldValueType, "field.FieldValueType");

			Guard.ThrowIfArgumentNotIs<Guid>(propertyType, "propertType");


			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			return (Guid?)value ?? Guid.Empty;
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			var guidValue = (Guid) value;
			if (guidValue == Guid.Empty)
				return null;
			
			return guidValue;
		}
	}
}