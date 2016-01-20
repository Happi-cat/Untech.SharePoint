using System;
using System.Collections.Generic;
using System.Globalization;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("Calculated")]
	public class CalculatedFieldConverter : MultiTypeFieldConverter
	{
		private static readonly IReadOnlyDictionary<string, Func<IFieldConverter>> ValueConverters = new Dictionary<string, Func<IFieldConverter>>
		{
			{"Text", () => new TextValueConverter()},
			{"Number", () => new NumberValueConverter()},
			{"Currency", () => new NumberValueConverter()},
			{"DateTime", () => new DateTimeValueConverter()},
			{"Boolean", () => new BoolValueConverter()}
		};

		public override void Initialize(MetaField field)
		{
			base.Initialize(field);

			if (!field.IsCalculated)
			{
				throw new ArgumentException("This field is not a calculated.");
			}

			if (!ValueConverters.ContainsKey(field.OutputType))
			{
				throw new ArgumentException(string.Format("Output type '{0}' is invalid.", field.OutputType));
			}

			Internal = ValueConverters[field.OutputType]();
		}

		private static string ExtractValue(string str, string expectedPrefix)
		{
			if (string.IsNullOrEmpty(str)) return null;
			if (str.StartsWith("error;#", StringComparison.InvariantCultureIgnoreCase))
			{
				throw new ArgumentException("Calculated field value is an error: " + str.Substring("error;#".Length));
			}
			if (str.StartsWith(expectedPrefix, StringComparison.InvariantCultureIgnoreCase))
			{
				return str.Substring(expectedPrefix.Length);
			}
			throw new ArgumentException(string.Format("Calculated field value '{0}' has an invalid type: ", str));
		}

		private class TextValueConverter : IFieldConverter 
		{
			public void Initialize(MetaField field)
			{
				if (field.MemberType != typeof (string))
				{
					throw new ArgumentException("Only string member type is allowed.");
				}
			}

			public object FromSpValue(object value)
			{
				return ExtractValue((string)value, "string;#");
			}

			public object ToSpValue(object value)
			{
				return "string;#" + (string)value;
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
				var strValue = ExtractValue((string)value, "float;#");
				if (string.IsNullOrEmpty(strValue)) return 0.0;
				return double.Parse(strValue, CultureInfo.InvariantCulture);
			}

			public object ToSpValue(object value)
			{
				return "float;#" + (double) value;
			}

			public string ToCamlValue(object value)
			{
				return ((double) value).ToString(CultureInfo.InvariantCulture);
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
				var strValue = ExtractValue((string)value, "datetime;#");
				if (string.IsNullOrEmpty(strValue)) return DateTime.MinValue;
				return DateTime.Parse(strValue, CultureInfo.InvariantCulture);
			}

			public object ToSpValue(object value)
			{
				return "datetime;#" + ((DateTime)value).ToString("u");
			}

			public string ToCamlValue(object value)
			{
				return ((DateTime) value).ToString("u");
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
				var strValue = ExtractValue((string)value, "boolean;#");
				if (string.IsNullOrEmpty(strValue)) return false;
				return strValue == "1";
			}

			public object ToSpValue(object value)
			{
				var boolValue = (bool) value;
				return "boolean;#" + (boolValue ? "1" : "0");
			}

			public string ToCamlValue(object value)
			{
				return (bool) value ? "1" : "0";
			}
		}
	}
}