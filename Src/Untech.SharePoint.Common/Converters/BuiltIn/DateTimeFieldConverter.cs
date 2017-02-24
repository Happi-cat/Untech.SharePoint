using System;
using System.Collections.Generic;
using System.Text;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("DateTime")]
	[UsedImplicitly]
	internal class DateTimeFieldConverter : MultiTypeFieldConverter
	{
		private static readonly IReadOnlyDictionary<Type, Func<IFieldConverter>> s_typeConverters = new Dictionary<Type, Func<IFieldConverter>>
		{
			{typeof(DateTime), () => new DateTimeTypeConverter()},
			{typeof(DateTime?), () => new NullableDateTimeTypeConverter()}
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
				throw new ArgumentException("Member type should be DateTime or Syste.Nullable<DateTime>.");
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

		private static DateTime ToLocalTime(DateTime dateTime)
		{
			return dateTime.Kind == DateTimeKind.Unspecified
				? new DateTime(dateTime.Ticks, DateTimeKind.Local)
				: dateTime.ToLocalTime();
		}

		private static DateTime? ToLocalTime(DateTime? dateTime)
		{
			return dateTime.HasValue
				? dateTime.Value.Kind == DateTimeKind.Unspecified
					? new DateTime(dateTime.Value.Ticks, DateTimeKind.Local)
					: dateTime.Value.ToLocalTime()
				: (DateTime?)null;
		}

		private class DateTimeTypeConverter : IFieldConverter
		{
			private static readonly DateTime s_min = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local);

			public void Initialize(MetaField field)
			{
			}

			public object FromSpValue(object value)
			{
				return ToLocalTime((DateTime?)value ?? s_min);
			}

			public object ToSpValue(object value)
			{
				var dateValue = ToLocalTime((DateTime)value);
				return dateValue > s_min ? dateValue : (object)null;
			}

			public string ToCamlValue(object value)
			{
				var dateValue = (DateTime)value;
				return dateValue > s_min ? CreateIsoDate(ToLocalTime(dateValue)) : "";
			}
		}

		private class NullableDateTimeTypeConverter : IFieldConverter
		{
			public void Initialize(MetaField field)
			{
			}

			public object FromSpValue(object value)
			{
				return ToLocalTime((DateTime?)value);
			}

			public object ToSpValue(object value)
			{
				return ToLocalTime((DateTime?)value);
			}

			public string ToCamlValue(object value)
			{
				var dateValue = (DateTime?)value;
				return dateValue.HasValue ? CreateIsoDate(ToLocalTime(dateValue.Value)) : "";
			}
		}
	}
}
