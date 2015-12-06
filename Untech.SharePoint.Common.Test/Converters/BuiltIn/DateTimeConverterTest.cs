using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.BuiltIn;

namespace Untech.SharePoint.Common.Test.Converters.BuiltIn
{
	[TestClass]
	public class DateTimeConverterTest : BaseConverterTest
	{
		private DateTime _value = DateTime.Now;

		[TestMethod]
		public void CanConvertDateTime()
		{
			Given<DateTime>()
				.CanConvertFromSp(_value, _value)
				.CanConvertFromSp(null, new DateTime(1900, 1, 1))
				.CanConvertToSp(_value, _value)
				.CanConvertToCaml(_value, _value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
		}

		[TestMethod]
		public void CanConvertNullableDateTime()
		{
			Given<DateTime?>()
				.CanConvertFromSp(_value, _value)
				.CanConvertFromSp(null, null)
				.CanConvertToSp(_value, _value)
				.CanConvertToSp(null, null)
				.CanConvertToCaml(_value, _value.ToString("yyyy-MM-ddTHH:mm:ssZ"))
				.CanConvertToCaml(null, "");
		}

		[TestMethod]
		public void NotSupportDouble()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<double>());
		}

		[TestMethod]
		public void NotSupportInt16()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<Int16>());
		}


		[TestMethod]
		public void NotSupportFloat()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<float>());
		}

		[TestMethod]
		public void NotSupportUInt32()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<UInt32>());
		}


		protected override IFieldConverter GetConverter()
		{
			return new DateTimeFieldConverter();
		}
	}
}