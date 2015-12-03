using System.Xml.Serialization;
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
				.CanConvertFromSp("<Test />", new TestObject())
				.CanConvertFromSp("<Test><Inner><Id>2</Id></Inner></Test>", new TestObject {Inner = new InnerObject {Id = 2}})
				.CanConvertFromSp("<Test Field='value'></Test>", new TestObject {Field = "value"})
				.CanConvertToSp(null, null)
				.CanConvertToSp(new TestObject(),
					"<Test xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" />")
				.CanConvertToSp(new TestObject {Field = "test"},
					"<Test xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Field=\"test\" />")
				.CanConvertToSp(new TestObject { Field = "test", Inner = new InnerObject { Id=3 }},
					"<Test xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Field=\"test\"><Inner><Id>3</Id></Inner></Test>");
		}

		[TestMethod]
		public void CanConvertNamespacedObject()
		{
			Given<NamespacedObject>()
				.CanConvertFromSp(null, null)
				.CanConvertFromSp("", null)
				.CanConvertFromSp("<Obj xmlns=\"http://namespace.my\" />", new NamespacedObject())
				.CanConvertFromSp("<Obj xmlns=\"http://namespace.my\" Field1='value'></Obj>", new NamespacedObject { Field1 = "value" })
				.CanConvertToSp(null, null)
				.CanConvertToSp(new NamespacedObject(),
					"<Obj xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://namespace.my\" />")
				.CanConvertToSp(new NamespacedObject {Field1 = "test", Field2 = "value"},
					"<Obj xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Field1=\"test\" Field2=\"value\" xmlns=\"http://namespace.my\" />");
		}

		protected override IFieldConverter GetConverter()
		{
			return new XmlFieldConverter();
		}

		[XmlRoot("Test", Namespace = "", DataType = "")]
		public class TestObject
		{
			[XmlAttribute]
			public string Field { get; set; }

			[XmlElement]
			public InnerObject Inner{ get; set; }


			public override bool Equals(object obj)
			{
				if (obj == null) return false;
				if (ReferenceEquals(this, obj)) return true;
				return obj.GetHashCode() == GetHashCode();
			}

			public override int GetHashCode()
			{
				var innerHash = Inner != null ? Inner.Id.GetHashCode() : 0;
				return (Field != null ? Field.GetHashCode() : 0) ^ innerHash;
			}
		}

		[XmlType(Namespace = "")]
		public class InnerObject 
		{
			[XmlElement]
			public int Id { get; set; }
		}

		[XmlRoot("Obj", Namespace = "http://namespace.my", DataType = "")]
		public class NamespacedObject
		{
			[XmlAttribute(Namespace = "http://namespace.my")]
			public string Field1 { get; set; }

			[XmlAttribute]
			public string Field2 { get; set; }

			public override bool Equals(object obj)
			{
				if (obj == null) return false;
				if (ReferenceEquals(this, obj)) return true;
				return obj.GetHashCode() == GetHashCode();
			}

			public override int GetHashCode()
			{
				return (Field1 != null ? Field1.GetHashCode() : 0) ^ (Field2 != null ? Field2.GetHashCode() : 0);
			}
		}
	}
}