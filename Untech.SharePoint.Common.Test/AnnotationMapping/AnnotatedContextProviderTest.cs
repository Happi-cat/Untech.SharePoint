using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	[TestClass]
	public class AnnotatedContextProviderTest
	{
		[TestMethod]
		public void CanCreateContextAndGetModel()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.IsNotNull(model);
		}

		[TestMethod]
		public void CanSeeOnlyAnnotatedContextPropertiesWithISpListType()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual(2, model.Lists.Count);
			Assert.AreEqual(2, model.Lists["List1"].ContentTypes.Count);
			Assert.AreEqual(1, model.Lists["List2"].ContentTypes.Count);
		}

		[TestMethod]
		public void CanSeeContentTypeId()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual("0x0010", model.Lists["List1"].ContentTypes[typeof(TestEntity)].ContentTypeId);
			Assert.AreEqual("0x001020", model.Lists["List1"].ContentTypes[typeof(DerivedTestEntity1)].ContentTypeId);
		}

		[TestMethod]
		public void CanInheritContentTypeAnnotation()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual("0x0010", model.Lists["List2"].ContentTypes[typeof(DerivedTestEntity2)].ContentTypeId);
		}

		[TestMethod]
		public void CanSeeAnnotatedProperties()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual(3, model.Lists["List1"].ContentTypes[typeof(TestEntity)].Fields.Count);
			Assert.AreEqual(4, model.Lists["List1"].ContentTypes[typeof(DerivedTestEntity1)].Fields.Count);
			Assert.AreEqual(4, model.Lists["List2"].ContentTypes[typeof(DerivedTestEntity2)].Fields.Count);
		}

		[TestMethod]
		public void CanInheritFieldAnnotation()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual("OldInternalName", model.Lists["List1"].ContentTypes[typeof(DerivedTestEntity1)].Fields["OverrideProperty"].FieldInternalName);
		}

		[TestMethod]
		public void CanOverwriteFieldAnnotation()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual("NewInternalName", model.Lists["List2"].ContentTypes[typeof(DerivedTestEntity2)].Fields["OverrideProperty"].FieldInternalName);
		}
	}
}
