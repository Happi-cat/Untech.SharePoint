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
			var model = new AnnotatedContextProvider<TestContext>().GetMetaContext();

			Assert.IsNotNull(model);
		}

		[TestMethod]
		public void CanSeeOnlyAnnotatedContextPropertiesWithISpListType()
		{
			var model = new AnnotatedContextProvider<TestContext>().GetMetaContext();

			Assert.AreEqual(2, model.Lists.Count);
			Assert.AreEqual(2, model.Lists.GetByListTitle("List1").ContentTypes.Count);
			Assert.AreEqual(1, model.Lists.GetByListTitle("List2").ContentTypes.Count);
		}

		[TestMethod]
		public void CanSeeContentTypeId()
		{
			var model = new AnnotatedContextProvider<TestContext>().GetMetaContext();

			Assert.AreEqual("0x0010", model.Lists.GetByListTitle("List1").ContentTypes.GetByEntityType(typeof(TestEntity)).ContentTypeId);
			Assert.AreEqual("0x001020", model.Lists.GetByListTitle("List1").ContentTypes.GetByEntityType(typeof(DerivedTestEntity1)).ContentTypeId);
		}

		[TestMethod]
		public void CanInheritContentTypeAnnotation()
		{
			var model = new AnnotatedContextProvider<TestContext>().GetMetaContext();

			Assert.AreEqual("0x0010", model.Lists.GetByListTitle("List2").ContentTypes.GetByEntityType(typeof(DerivedTestEntity2)).ContentTypeId);
		}

		[TestMethod]
		public void CanSeeAnnotatedProperties()
		{
			var model = new AnnotatedContextProvider<TestContext>().GetMetaContext();

			Assert.AreEqual(3, model.Lists.GetByListTitle("List1").ContentTypes.GetByEntityType(typeof(TestEntity)).Fields.Count);
			Assert.AreEqual(4, model.Lists.GetByListTitle("List1").ContentTypes.GetByEntityType(typeof(DerivedTestEntity1)).Fields.Count);
			Assert.AreEqual(4, model.Lists.GetByListTitle("List2").ContentTypes.GetByEntityType(typeof(DerivedTestEntity2)).Fields.Count);
		}

		[TestMethod]
		public void CanInheritFieldAnnotation()
		{
			var model = new AnnotatedContextProvider<TestContext>().GetMetaContext();

			Assert.AreEqual("OldInternalName", model.Lists.GetByListTitle("List1").ContentTypes.GetByEntityType(typeof(DerivedTestEntity1)).Fields.GetByMemberName("OverrideProperty").FieldInternalName);
		}

		[TestMethod]
		public void CanOverwriteFieldAnnotation()
		{
			var model = new AnnotatedContextProvider<TestContext>().GetMetaContext();

			Assert.AreEqual("NewInternalName", model.Lists.GetByListTitle("List2").ContentTypes.GetByEntityType(typeof(DerivedTestEntity2)).Fields.GetByMemberName("OverrideProperty").FieldInternalName);
		}
	}
}
