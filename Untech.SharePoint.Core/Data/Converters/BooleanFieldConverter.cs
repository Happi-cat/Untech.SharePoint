using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Boolean")]
	public class BooleanFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(bool))
				throw new ArgumentException("SPField with bool value type only supported");

			if (propertyType != typeof(bool) && propertyType != typeof(bool?))
				throw new ArgumentException("This converter can be used only with bool or Nullable<bool> property types");

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