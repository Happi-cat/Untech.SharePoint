using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
{
	[SpFieldConverter("Calculated")]
	internal class CalculatedFieldConverter : MultiTypeFieldConverter
	{
		private static readonly IReadOnlyDictionary<string, Func<IFieldConverter>> s_valueConverters = new Dictionary
			<string, Func<IFieldConverter>>
		{
			["Text"] = () => new TextValueConverter(),
			["Number"] = () => new NumberValueConverter(),
			["Currency"] = () => new NumberValueConverter(),
			["DateTime"] = () => new DateTimeValueConverter(),
			["Boolean"] = () => new BoolValueConverter()
		};

		public override void Initialize(MetaField field)
		{
			base.Initialize(field);

			if (!field.IsCalculated)
			{
				throw new ArgumentException("This fields is not a calculated.");
			}

			if (!s_valueConverters.ContainsKey(field.OutputType))
			{
				throw new ArgumentException($"Output type '{field.OutputType}' is invalid.");
			}

			Internal = s_valueConverters[field.OutputType]();
		}

		private static T GetValue<T>(object value)
		{
			if (value == null) return default(T);

			var errValue = value as FieldCalculatedErrorValue;
			if (errValue != null)
			{
				throw new ArgumentException("Calculated field value is an error: " + errValue.ErrorMessage);
			}

			Guard.CheckIsObjectAssignableTo<T>(nameof(value), value);
			return (T)value;
		}

		private class TextValueConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{
				if (field.MemberType != typeof(string))
				{
					throw new ArgumentException("Only string member type is allowed.");
				}
			}

			public object FromSpValue(object value)
			{
				return GetValue<string>(value);
			}

			public object ToSpValue(object value)
			{
				return (string)value;
			}

			public string ToCamlValue(object value)
			{
				return (string)value;
			}
		}

		private class NumberValueConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{
				if (field.MemberType != typeof(double))
				{
					throw new ArgumentException("Only double member type is allowed.");
				}
			}

			public object FromSpValue(object value)
			{
				return GetValue<double>(value);
			}

			public object ToSpValue(object value)
			{
				return (double)value;
			}

			public string ToCamlValue(object value)
			{
				return ((double)value).ToString(CultureInfo.InvariantCulture);
			}
		}

		private class DateTimeValueConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{
				if (field.MemberType != typeof(DateTime))
				{
					throw new ArgumentException("Only DateTime member type is allowed.");
				}
			}

			public object FromSpValue(object value)
			{
				return GetValue<DateTime>(value);
			}

			public object ToSpValue(object value)
			{
				return (DateTime)value;
			}

			public string ToCamlValue(object value)
			{
				return ((DateTime)value).ToString("u");
			}
		}

		private class BoolValueConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{
				if (field.MemberType != typeof(bool))
				{
					throw new ArgumentException("Only bool member type is allowed.");
				}
			}

			public object FromSpValue(object value)
			{
				var strValue = GetValue<string>(value);
				if (string.IsNullOrEmpty(strValue)) return false;
				return strValue == "1";
			}

			public object ToSpValue(object value)
			{
				return (bool)value ? "1" : "0";
			}

			public string ToCamlValue(object value)
			{
				return (bool)value ? "1" : "0";
			}
		}
	}
}