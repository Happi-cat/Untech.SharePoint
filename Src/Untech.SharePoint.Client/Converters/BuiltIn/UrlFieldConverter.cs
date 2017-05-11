using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Converters;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.Models;

namespace Untech.SharePoint.Client.Converters.BuiltIn
{
	[SpFieldConverter("URL")]
	[UsedImplicitly]
	internal class UrlFieldConverter : MultiTypeFieldConverter
	{
		private static readonly IReadOnlyDictionary<Type, Func<IFieldConverter>> s_typeConverters = new Dictionary<Type, Func<IFieldConverter>>
		{
			[typeof(string)] = () => new StringTypeConverter(),
			[typeof(UrlInfo)] = () => new UrlInfoTypeConverter(),
		};

		public override void Initialize(MetaField field)
		{
			base.Initialize(field);
			if (s_typeConverters.ContainsKey(field.MemberType))
			{
				Internal = s_typeConverters[field.MemberType]();
			}
			else
			{
				throw new ArgumentException("MemberType is invalid");
			}
		}

		private class StringTypeConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{
			}

			public object FromSpValue(object value)
			{
				return ((FieldUrlValue)value)?.Url;
			}

			public object ToSpValue(object value)
			{
				return value == null ? null : new FieldUrlValue { Url = value.ToString() };
			}

			public string ToCamlValue(object value)
			{
				return (string)value ?? "";
			}
		}

		private class UrlInfoTypeConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{
			}

			public object FromSpValue(object value)
			{
				if (value == null)
					return null;

				var spValue = (FieldUrlValue)value;

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

				var urlInfo = (UrlInfo)value;

				return new FieldUrlValue { Url = urlInfo.Url, Description = urlInfo.Description };
			}

			public string ToCamlValue(object value)
			{
				if (value == null)
					return "";

				var urlInfo = (UrlInfo)value;
				if (string.IsNullOrEmpty(urlInfo.Description))
				{
					return urlInfo.Url;
				}
				return string.Format("{0}, {1}", urlInfo.Url.Replace(",", ",,"), urlInfo.Description);
			}
		}
	}
}