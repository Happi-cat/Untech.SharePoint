using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Test.AnnotationMapping.Models;
using TestContext = Untech.SharePoint.Common.Test.AnnotationMapping.Models.TestContext;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	[TestClass]
	public class AnnotatedContentTypeMappingTest
	{
		[TestMethod]
		public void CanSeeContentTypeId()
		{
			var model = GetCtx<TestContext>();

			Assert.AreEqual("0x0010", GetContenType<TestEntity>(model, "List1").Id);
			Assert.AreEqual("0x001020", GetContenType<DerivedTestEntityWithIheritedAnnotation>(model, "List1").Id);
		}

		[TestMethod]
		public void CanInheritContentTypeAnnotation()
		{
			var model = GetCtx<TestContext>();

			Assert.AreEqual("0x0010", GetContenType<DerivedTestEntityWithOverwrittenAnnotation>(model, "List2").Id);
		}

		[TestMethod]
		public void CanSeeOnlyAnnotatedEntityProperties()
		{
			var model = GetCtx<TestContext>();

			Assert.AreEqual(4, GetContenType<TestEntity>(model, "List1").Fields.Count);
			Assert.AreEqual(5, GetContenType<DerivedTestEntityWithIheritedAnnotation>(model, "List1").Fields.Count);
			Assert.AreEqual(5, GetContenType<DerivedTestEntityWithOverwrittenAnnotation>(model, "List2").Fields.Count);
		}

		private MetaContext GetCtx<T>()
		{
			return new AnnotatedContextMapping<T>().GetMetaContext();
		}

		private MetaContentType GetContenType<T>(MetaContext context, string list)
		{
			return context.Lists[list].ContentTypes[typeof(T)];
		}
	}
}