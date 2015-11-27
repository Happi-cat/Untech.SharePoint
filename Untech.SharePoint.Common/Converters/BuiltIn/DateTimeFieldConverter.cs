using System;
using System.Text;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("DateTime")]
	[UsedImplicitly]
	internal class DateTimeFieldConverter : MultiTypeFieldConverter
	{
		public override void Initialize(MetaField field)
		{
			base.Initialize(field);
			if (field.MemberType == typeof(bool))
			{
				Internal = new DateTimeConverter();
			}
			else if (field.MemberType == typeof(bool?))
			{
				Internal = new NullableDateTimeConverter();
			}
			else
			{
				throw new ArgumentException("MemberType is invalid");
			}
		}

		/// <summary>
		/// Converts a system DateTime value to ISO8601 DateTime format (yyyy-mm-ddThh:mm:ssZ).
		/// </summary>
		/// <returns>
		/// A string that contains the date and time in ISO8601 DateTime format.
		/// </returns>
		/// <param name="dtValue">A System.DateTime object that represents the system DateTime value in the form mm/dd/yyyy hh:mm:ss AM or PM.</param>
		private static string CreateIsoDate(DateTime dtValue)
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

		private class DateTimeConverter : IFieldConverter
		{
			private static readonly DateTime Min = new DateTime(1900, 1, 1);

			public void Initialize(MetaField field)
			{
				
			}

			public object FromSpValue(object value)
			{
				return (DateTime?) value ?? Min;
			}

			public object ToSpValue(object value)
			{
				var dateValue = (DateTime)value;
				return dateValue > Min ? dateValue : (object) null;
			}

			public string ToCamlValue(object value)
			{
				var dateValue = (DateTime)value;
				return dateValue > Min ? CreateIsoDate(dateValue) : null;
			}
		}

		private class NullableDateTimeConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{

			}

			public object FromSpValue(object value)
			{
				return (DateTime?)value;
			}

			public object ToSpValue(object value)
			{
				return (DateTime?)value;
			}

			public string ToCamlValue(object value)
			{
				var dateValue = (DateTime?)value;
				return dateValue.HasValue ? CreateIsoDate(dateValue.Value) : null;
			}
		}
	}
}
