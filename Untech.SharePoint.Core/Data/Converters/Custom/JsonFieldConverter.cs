using System;
using Microsoft.SharePoint;
using Newtonsoft.Json;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters.Custom
{
	public class JsonFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.ThrowIfArgumentNull(field, "field");
			Guard.ThrowIfArgumentNull(propertyType, "propertyType");

			Guard.ThrowIfArgumentNotIs<string>(field.FieldValueType, "field.FieldValueType");

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