using System;
using Microsoft.SharePoint;
using Newtonsoft.Json;
namespace Untech.SharePoint.Core.Data.Fields.Converters
{
	public class JsonFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			return JsonConvert.DeserializeObject((string)value, propertyType);
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			return JsonConvert.SerializeObject(value);
		}
	}
}