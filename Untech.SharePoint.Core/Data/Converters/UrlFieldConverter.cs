using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("URL")]
	internal class UrlFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			Guard.ThrowIfNot<SPFieldUrl>(Field, "This Field Converter doesn't support that SPField type");

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
			Guard.ThrowIfNot<SPFieldUrl>(Field, "This Field Converter doesn't support that SPField type");

			if (value == null)
				return null;

			if (PropertyType == typeof(string))
			{
				return new SPFieldUrlValue(value.ToString());
			}

			var urlInfo = (UrlInfo) value;

			return new SPFieldUrlValue(string.Format("{0};#{1}", urlInfo.Url, urlInfo.Title));
		}
	}
}