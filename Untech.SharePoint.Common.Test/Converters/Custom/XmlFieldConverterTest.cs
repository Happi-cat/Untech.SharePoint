using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.Custom;

namespace Untech.SharePoint.Common.Test.Converters.Custom
{
	[TestClass]
	public class XmlFieldConverterTest : BaseConverterTest
	{
		[TestMethod]
		public void CanConvertTestObject()
		{
			Given<TestObject>()
				.CanConvertFromSp(null, null)
				.CanConvertFromSp("", null)
				.CanConvertFromSp("<Test xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" />", new TestObject())
				.CanConvertFromSp("<Test xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><Field>value</Field></Test>", new TestObject { Field = "value" })
				.CanConvertToSp(null, null)
				.CanConvertToSp(new TestObject(), "<Test xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" />")
				.CanConvertToSp(new TestObject { Field = "test" }, "<Test xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><Field>test</Field></Test>");
		}

		protected override IFieldConverter GetConverter()
		{
			return new XmlFieldConverter();
		}

		[DataContract(Name = "Test", Namespace = "")]
		public class TestObject
		{
			[DataMember(Name = "Field", EmitDefaultValue = false)]
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