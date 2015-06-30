using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SpFieldConverter("URL")]
	internal class UrlFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.ThrowIfArgumentNull(field, "field");
			Guard.ThrowIfArgumentNull(propertyType, "propertyType");
			
			Guard.ThrowIfArgumentNotIs<SPFieldUrlValue>(field.FieldValueType, "field.FieldValueType");
			
			Guard.ThrowIfArgumentNotIs(propertyType, new [] {typeof(UrlInfo), typeof(string)}, "propertType");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			if (PropertyType == typeof (string))
			{
				return new UrlInfo(new SPFieldUrlValue(value.ToString())).Url;
			}

			return new UrlInfo(new SPFieldUrlValue(value.ToString()));
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			if (PropertyType == typeof(string))
			{
				return new SPFieldUrlValue(value.ToString());
			}

			var urlInfo = (UrlInfo) value;

			return new SPFieldUrlValue(string.Format("{0};#{1}", urlInfo.Url, urlInfo.Description));
		}
	}
}