using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[TestClass]
	public class DateTimeConverterTest : BaseConverterTest
	{
		private DateTime _value = DateTime.Now;

		[TestMethod]
		public void CanConvertDateTime()
		{
			CreateConverterForFieldWithType<DateTime>()
				.CanConvertFromSp(_value, _value)
				.CanConvertFromSp(null, new DateTime(1900, 1, 1))
				.CanConvertToSp(_value, _value)
				.CanConvertToCaml(_value, _value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
		}

		[TestMethod]
		public void CanConvertNullableDateTime()
		{
			CreateConverterForFieldWithType<DateTime?>()
				.CanConvertFromSp(_value, _value)
				.CanConvertFromSp(null, null)
				.CanConvertToSp(_value, _value)
				.CanConvertToSp(null, null)
				.CanConvertToCaml(_value, _value.ToString("yyyy-MM-ddTHH:mm:ssZ"))
				.CanConvertToCaml(null, "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Init_ThrowNotSupported_WhenDouble()
		{
			CreateConverterForFieldWithType<double>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Init_ThrowNotSupported_WhenInt16()
		{
			CreateConverterForFieldWithType<Int16>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Init_ThrowNotSupported_WhenFloat()
		{
			CreateConverterForFieldWithType<float>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Init_ThrowNotSupported_WhenUInt32()
		{
			CreateConverterForFieldWithType<UInt32>();
		}

		protected override IFieldConverter GetConverter()
		{
			return new DateTimeFieldConverter();
		}
	}
}