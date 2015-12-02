using System.Diagnostics.CodeAnalysis;
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
				.CanConvertFromSp("{}", new TestObject())
				.CanConvertFromSp("{ \"Field\": \"value\" }", new TestObject { Field = "value"})
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

			public override bool Equals(object obj)
			{
				if (obj == null) return false;
				if (ReferenceEquals(this, obj)) return true;
				return obj.GetHashCode() == GetHashCode();
			}

			public override int GetHashCode()
			{
				return Field != null ? Field.GetHashCode() : 0;
			}
		}
	}
}