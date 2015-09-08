using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	[TestClass]
	public class AnnotatedContextMappingTest
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

			Assert.AreEqual("0x0010", GetContenType<TestEntity>(model, "List1").ContentTypeId);
			Assert.AreEqual("0x001020", GetContenType<DerivedTestEntity1>(model, "List1").ContentTypeId);
		}

		[TestMethod]
		public void CanInheritContentTypeAnnotation()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual("0x0010", GetContenType<DerivedTestEntity2>(model, "List2").ContentTypeId);
		}

		[TestMethod]
		public void CanSeeAnnotatedProperties()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual(3, GetContenType<TestEntity>(model, "List1").Fields.Count);
			Assert.AreEqual(4, GetContenType<DerivedTestEntity1>(model, "List1").Fields.Count);
			Assert.AreEqual(4, GetContenType<DerivedTestEntity2>(model, "List2").Fields.Count);
		}

		[TestMethod]
		public void CanInheritFieldAnnotation()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual("OldInternalName", GetContenType<DerivedTestEntity1>(model, "List1").Fields["OverrideProperty"].FieldInternalName);
		}

		[TestMethod]
		public void CanOverwriteFieldAnnotation()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual("NewInternalName", GetContenType<DerivedTestEntity2>(model, "List2").Fields["OverrideProperty"].FieldInternalName);
		}

		private MetaContentType GetContenType<T>(MetaContext context, string list)
		{
			return context.Lists[list].ContentTypes[typeof(T)];
		}
	}
}
