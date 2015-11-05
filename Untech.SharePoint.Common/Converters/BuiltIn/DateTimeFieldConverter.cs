using System;
using System.Text;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("DateTime")]
	internal class DateTimeFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			if (Field.MemberType == typeof(DateTime?))
				return (DateTime?)value;

			return (DateTime?)value ?? new DateTime(1900, 1, 1);
		}

		public object ToSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			var dateValue = (DateTime)value;
			if (dateValue <= new DateTime(1900, 1, 1))
			{
				return null;
			}

			return dateValue;
		}

		public string ToCamlValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			var dateValue = (DateTime)value;
			if (dateValue <= new DateTime(1900, 1, 1))
			{
				return null;
			}

			return CreateISO8601DateTime(dateValue);
		}

		/// <summary>
		/// Converts a system DateTime value to ISO8601 DateTime format (yyyy-mm-ddThh:mm:ssZ).
		/// </summary>
		/// <returns>
		/// A string that contains the date and time in ISO8601 DateTime format.
		/// </returns>
		/// <param name="dtValue">A System.DateTime object that represents the system DateTime value in the form mm/dd/yyyy hh:mm:ss AM or PM.</param>
		public static string CreateISO8601DateTime(DateTime dtValue)
		{
			var sb = new StringBuilder();
			sb.Append(dtValue.Year.ToString("0000"));
			sb.Append("-");
			sb.Append(dtValue.Month.ToString("00"));
			sb.Append("-");
			sb.Append(dtValue.Day.ToString("00"));
			sb.Append("T");
			sb.Append(dtValue.Hour.ToString("00"));
			sb.Append(":");
			sb.Append(dtValue.Minute.ToString("00"));
			sb.Append(":");
			sb.Append(dtValue.Second.ToString("00"));
			sb.Append("Z");
			return sb.ToString();
		}
	}
}
