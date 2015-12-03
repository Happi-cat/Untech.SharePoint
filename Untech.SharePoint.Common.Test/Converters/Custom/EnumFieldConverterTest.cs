using System;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.Custom;

namespace Untech.SharePoint.Common.Test.Converters.Custom
{
	[TestClass]
	public class EnumFieldConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanConvertTestEnum()
		{
			Given<TestEnum>()
				.CanConvertFromSp("Default", TestEnum.Default)
				.CanConvertFromSp("Option1", TestEnum.Option1)
				.CanConvertFromSp("Option2", TestEnum.Option2)
				.CanConvertFromSp("Option 2", TestEnum.Option2)
				.CanConvertFromSp(null, TestEnum.Default)
				.CanConvertToSp(TestEnum.Default, "Default")
				.CanConvertToSp(TestEnum.Option2, "Option 2")
				.CanConvertToCaml(TestEnum.Option1, "Option1")
				.CanConvertToCaml(TestEnum.Option2, "Option 2");
		}

		[TestMethod]
		public void CanConvertNullableTestEnum()
		{
			Given<TestEnum?>()
				.CanConvertFromSp("Default", TestEnum.Default)
				.CanConvertFromSp("Option1", TestEnum.Option1)
				.CanConvertFromSp("Option2", TestEnum.Option2)
				.CanConvertFromSp("Option 2", TestEnum.Option2)
				.CanConvertFromSp(null, null)
				.CanConvertToSp(TestEnum.Default, "Default")
				.CanConvertToSp(TestEnum.Option2, "Option 2")
				.CanConvertToSp(null, null)
				.CanConvertToCaml(TestEnum.Option1, "Option1")
				.CanConvertToCaml(TestEnum.Option2, "Option 2")
				.CanConvertToCaml(null, "");
		}

		[TestMethod]
		public void NotSupportEnumWithoutDefault()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<NoDefaultEnum>());
		}


		[TestMethod]
		public void NotSupportInt()
		{
			CustomAssert.Throw<ArgumentException>(() => Given<int>());
		}

		protected override IFieldConverter GetConverter()
		{
			return new EnumFieldConverter();
		}

		public enum TestEnum
		{
			Default = 0,
			Option1,
			[EnumMember(Value = "Option 2")]
			Option2
		}

		public enum NoDefaultEnum
		{
			Default = 1,
			Option1,
			[EnumMember(Value = "Option 2")]
			Option2
		}
	}
}
