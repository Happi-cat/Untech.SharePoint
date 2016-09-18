using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.Custom;

namespace Untech.SharePoint.Common.Test.Converters.Custom
{
	[TestClass]
	public class JsonFieldConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanConvertTestObject()
		{
			Given<TestObject>()
				.CanConvertFromSp(null, null)
				.CanConvertFromSp("", null)
				.CanConvertFromSp("{}", new TestObject(), new TestObjectComparer())
				.CanConvertFromSp("{ \"Field\": \"value\" }", new TestObject { Field = "value"}, new TestObjectComparer())
				.CanConvertToSp(null, null)
				.CanConvertToSp(new TestObject(), "{\"Field\":null}")
				.CanConvertToSp(new TestObject { Field = "test" }, "{\"Field\":\"test\"}");

		}

		protected override IFieldConverter GetConverter()
		{
			return new JsonFieldConverter();
		}

		public class TestObject
		{
			[DataMember]
			public string Field { get; set; }
		}

		public class TestObjectComparer : EqualityComparer<TestObject>
		{
			public override bool Equals(TestObject x, TestObject y)
			{
				return x.Field == y.Field;
			}

			public override int GetHashCode(TestObject obj)
			{
				if (obj == null) return 0;
				var hash1 = (obj.Field ?? "").GetHashCode();
				return hash1;
			}
		}
	}
}