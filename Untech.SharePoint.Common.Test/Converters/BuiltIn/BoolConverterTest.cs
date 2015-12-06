using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.BuiltIn;

namespace Untech.SharePoint.Common.Test.Converters.BuiltIn
{
	[TestClass]
	public class BoolConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void NotSupportInteger()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<int>());
		}

		[TestMethod]
		public void NotSupportNullableInteger()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<int?>());
		}

		[TestMethod]
		public void CanConvertBool()
		{
			Given<bool>()
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
			Given<bool?>()
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