using System.Collections.Generic;
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
				.CanConvertFromSp("<Test />", new TestObject(), new TestObjectComparer())
				.CanConvertFromSp("<Test><Inner><Id>2</Id></Inner></Test>", new TestObject {Inner = new InnerObject {Id = 2}}, new TestObjectComparer())
				.CanConvertFromSp("<Test Field='value'></Test>", new TestObject {Field = "value"}, new TestObjectComparer())
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
				.CanConvertFromSp("<Obj xmlns=\"http://namespace.my\" />", new NamespacedObject(), new NamespacedObjectComparer())
				.CanConvertFromSp("<Obj xmlns=\"http://namespace.my\" Field1='value'></Obj>", new NamespacedObject { Field1 = "value" }, new NamespacedObjectComparer())
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
			public InnerObject Inner { get; set; }
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
		}

		public class TestObjectComparer : EqualityComparer<TestObject>
		{
			public override bool Equals(TestObject x, TestObject y)
			{
				return x.Field == y.Field && 
					((x.Inner != null && y.Inner != null && x.Inner.Id == y.Inner.Id) || (x.Inner == null && y.Inner == null));
			}

			public override int GetHashCode(TestObject obj)
			{
				if (obj == null) return 0;
				var hash1 = (obj.Field ?? "").GetHashCode();
				var hash2 = obj.Inner != null ? obj.Inner.GetHashCode() : 0;
				return hash1 ^ hash2;
			}
		}

		public class NamespacedObjectComparer : EqualityComparer<NamespacedObject>
		{
			public override bool Equals(NamespacedObject x, NamespacedObject y)
			{
				return x.Field1 == y.Field1 && x.Field2 == y.Field2;
			}

			public override int GetHashCode(NamespacedObject obj)
			{
				if (obj == null) return 0;
				var hash1 = (obj.Field1 ?? "").GetHashCode();
				var hash2 = (obj.Field2 ?? "").GetHashCode();
				return hash1 ^ hash2;
			}
		}
	}
}