using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[TestClass]
	public class BoolConverterTest : BaseConverterTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotSupportInteger()
		{
			CreateConverterForFieldWithType<int>();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NotSupportNullableInteger()
		{
			CreateConverterForFieldWithType<int?>();
		}

		[TestMethod]
		public void CanConvertBool()
		{
			CreateConverterForFieldWithType<bool>()
				.CanConvertFromSp(true, true)
				.CanConvertFromSp(false, false)
				.CanConvertFromSp(null, false)
				.CanConvertToSp(true, true)
				.CanConvertToSp(false, false)
				.CanConvertToCaml(true, "1")
				.CanConvertToCaml(false, "0");
		}

		[TestMethod]
		public void CanConvertNullableBool()
		{
			CreateConverterForFieldWithType<bool?>()
				.CanConvertFromSp(true, true)
				.CanConvertFromSp(false, false)
				.CanConvertFromSp(null, null)
				.CanConvertToSp(true, true)
				.CanConvertToSp(false, false)
				.CanConvertToSp(null, null)
				.CanConvertToCaml(true, "1")
				.CanConvertToCaml(false, "0");
		}

		protected override IFieldConverter GetConverter()
		{
			return new BooleanFieldConverter();
		}
	}
}