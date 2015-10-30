using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Test.Mappings.Annotation.Models;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation
{
	[TestClass]
	public class AnnotatedContentTypeMappingTest
	{
		[TestMethod]
		public void CanSeeContentTypeId()
		{
			var model = GetCtx<Ctx>();

			Assert.AreEqual("0x0010", GetContenType<Entity>(model, "List1").Id);
			Assert.AreEqual("0x001020", GetContenType<DerivedEntityWithIheritedAnnotation>(model, "List1").Id);
		}

		[TestMethod]
		public void CanInheritContentTypeAnnotation()
		{
			var model = GetCtx<Ctx>();

			Assert.AreEqual("0x0010", GetContenType<DerivedEntityWithOverwrittenAnnotation>(model, "List2").Id);
		}

		[TestMethod]
		public void CanSeeOnlyAnnotatedEntityProperties()
		{
			var model = GetCtx<Ctx>();

			Assert.AreEqual(4, GetContenType<Entity>(model, "List1").Fields.Count);
			Assert.AreEqual(5, GetContenType<DerivedEntityWithIheritedAnnotation>(model, "List1").Fields.Count);
			Assert.AreEqual(5, GetContenType<DerivedEntityWithOverwrittenAnnotation>(model, "List2").Fields.Count);
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