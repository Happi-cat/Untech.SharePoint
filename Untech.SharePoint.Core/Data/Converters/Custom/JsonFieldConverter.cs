using System;
using Microsoft.SharePoint;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Data.Converters.Custom
{
	public class JsonFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(string))
				throw new ArgumentException("SPField with string value type only supported");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			return string.IsNullOrEmpty((string) value) ? null : JsonConvert.DeserializeObject((string)value, PropertyType);
		}

		public object ToSpValue(object value)
		{
			return value == null ? null : JsonConvert.SerializeObject(value);
		}
	}
}