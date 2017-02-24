using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.BuiltIn;

namespace Untech.SharePoint.Common.Test.Converters.BuiltIn
{
	[TestClass]
	public class NumberConverterTest : BaseConverterTest
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
		public void NotSupportFloat()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<float>());
		}

		[TestMethod]
		public void NotSupportNullableFloat()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<float?>());
		}

		[TestMethod]
		public void CanConvertDouble()
		{
			Given<double>()
				.CanConvertFromSp(1.0, 1.0)
				.CanConvertFromSp(null, 0.0)
				.CanConvertToSp(1.0, 1.0)
				.CanConvertToCaml(1.0, "1");
		}

		[TestMethod]
		public void CanConvertNullableDouble()
		{
			Given<double?>()
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