using System;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
{
	[SpFieldConverter("URL")]
	internal class UrlFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		/// <summary>
		/// Initialzes current instance with the specified <see cref="MetaField"/>
		/// </summary>
		/// <param name="field"></param>
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}


		public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			var spValue = (FieldUrlValue) value;

			if (Field.MemberType == typeof (string))
			{
				return spValue.Url;
			}

			return new UrlInfo
			{
				Url = spValue.Url,
				Description = spValue.Description
			};
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			if (Field.MemberType == typeof(string))
			{
				return new FieldUrlValue
				{
					Url = value.ToString()
				};
			}

			var urlInfo = (UrlInfo) value;

			return new FieldUrlValue
			{
				Url = urlInfo.Url,
				Description = urlInfo.Description
			};
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP Caml value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Caml value.</returns>
		public string ToCamlValue(object value)
		{
			var urlInfo = (FieldUrlValue)ToSpValue(value);
			if (urlInfo != null)
			{
				return string.Format("{0};#{1}", urlInfo.Url, urlInfo.Description);
			}

			return "";
		}
	}
}