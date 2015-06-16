using System;
using Microsoft.SharePoint;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Data.Converters
{
	public class JsonFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			if (string.IsNullOrEmpty((string) value))
			{
				return null;
			}

			return JsonConvert.DeserializeObject((string)value, propertyType);
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			if (value == null)
			{
				return null;
			}

			return JsonConvert.SerializeObject(value);
		}
	}
}