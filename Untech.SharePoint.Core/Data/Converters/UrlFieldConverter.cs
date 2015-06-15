using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("URL")]
	internal class UrlFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldUrl>(field, "This Field Converter doesn't support that SPField type");

			if (value == null)
				return null;

			if (propertyType == typeof (string))
			{
				return new UrlInfo(new SPFieldUrlValue(value.ToString())).Url;
			}

			return new UrlInfo(new SPFieldUrlValue(value.ToString()));
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldUrl>(field, "This Field Converter doesn't support that SPField type");

			if (value == null)
				return null;

			if (propertyType == typeof(string))
			{
				return new SPFieldUrlValue(value.ToString());
			}

			var urlInfo = (UrlInfo) value;

			return new SPFieldUrlValue(string.Format("{0};#{1}", urlInfo.Url, urlInfo.Title));
		}
	}
}