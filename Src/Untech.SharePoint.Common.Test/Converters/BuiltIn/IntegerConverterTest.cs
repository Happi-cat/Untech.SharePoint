using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.BuiltIn;

namespace Untech.SharePoint.Common.Test.Converters.BuiltIn
{
	[TestClass]
	public class IntegerConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanConvertInt32()
		{
			Given<Int32>()
				.CanConvertFromSp(1, 1)
				.CanConvertFromSp(null, 0)
				.CanConvertToSp(1, 1)
				.CanConvertToCaml(1, "1");
		}

		[TestMethod]
		public void CanConvertNullableInt32()
		{
			Given<Int32?>()
				.CanConvertFromSp(1, 1)
				.CanConvertFromSp(null, null)
				.CanConvertToSp(1, 1)
				.CanConvertToSp(null, null)
				.CanConvertToCaml(1, "1")
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
			return new IntegerFieldConverter();
		}
	}
}