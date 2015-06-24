using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("URL")]
	internal class UrlFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(SPFieldUrlValue))
				throw new ArgumentException("SPField with bool value type only supported");

			if (!propertyType.In(new [] {typeof(UrlInfo), typeof(string)}))
				throw new ArgumentException("This converter can be used only with UrlInfo or string property types");

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