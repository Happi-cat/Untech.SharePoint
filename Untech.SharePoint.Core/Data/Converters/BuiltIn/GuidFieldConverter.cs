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
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(Guid))
				throw new ArgumentException("SPField with Guid value type only supported");

			if (propertyType != typeof(Guid))
				throw new ArgumentException("This converter can be used only with Guid property types");


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