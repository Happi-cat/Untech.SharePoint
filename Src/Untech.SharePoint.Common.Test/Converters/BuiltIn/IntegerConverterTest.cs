using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[TestClass]
	public class IntegerConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanConvertInt32()
		{
			CreateConverterForFieldWithType<Int32>()
				.CanConvertFromSp(1, 1)
				.CanConvertFromSp(null, 0)
				.CanConvertToSp(1, 1)
				.CanConvertToCaml(1, "1");
		}

		[TestMethod]
		public void CanConvertNullableInt32()
		{
			CreateConverterForFieldWithType<Int32?>()
				.CanConvertFromSp(1, 1)
				.CanConvertFromSp(null, null)
				.CanConvertToSp(1, 1)
				.CanConvertToSp(null, null)
				.CanConvertToCaml(1, "1")
				.CanConvertToCaml(null, "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotSupportDouble()
		{
			CreateConverterForFieldWithType<double>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotSupportInt16()
		{
			CreateConverterForFieldWithType<Int16>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotSupportFloat()
		{
			CreateConverterForFieldWithType<float>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotSupportUInt32()
		{
			CreateConverterForFieldWithType<UInt32>();
		}

		protected override IFieldConverter GetConverter()
		{
			return new IntegerFieldConverter();
		}
	}
}