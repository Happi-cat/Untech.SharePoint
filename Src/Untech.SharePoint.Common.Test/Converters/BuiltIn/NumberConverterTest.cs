using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Converters.BuiltIn
{
	[TestClass]
	public class NumberConverterTest : BaseConverterTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Init_ThrowNotSupported_WhenInteger()
		{
			CreateConverterForFieldWithType<int>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Init_ThrowNotSupported_WhenNullableInteger()
		{
			CreateConverterForFieldWithType<int?>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Init_ThrowNotSupported_WhenFloat()
		{
			CreateConverterForFieldWithType<float>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Init_ThrowNotSupported_WhenNullableFloat()
		{
			CreateConverterForFieldWithType<float?>();
		}

		[TestMethod]
		public void CanConvertDouble()
		{
			CreateConverterForFieldWithType<double>()
				.CanConvertFromSp(1.0, 1.0)
				.CanConvertFromSp(null, 0.0)
				.CanConvertToSp(1.0, 1.0)
				.CanConvertToCaml(1.0, "1");
		}

		[TestMethod]
		public void CanConvertNullableDouble()
		{
			CreateConverterForFieldWithType<double?>()
				.CanConvertFromSp(1.0, 1.0)
				.CanConvertFromSp(null, null)
				.CanConvertToSp(1.0, 1.0)
				.CanConvertToSp(null, null)
				.CanConvertToCaml(1.0, "1")
				.CanConvertToCaml(null, "");
		}

		protected override IFieldConverter GetConverter()
		{
			return new NumberFieldConverter();
		}
	}
}