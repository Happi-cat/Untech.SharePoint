using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Test.Mappings.Annotation.Models;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation
{
	[TestClass]
	public class AnnotatedContextMappingTest
	{	
		[TestMethod]
		public void CanCreateContextAndGetModel()
		{
			var model = GetCtx<Ctx>();

			Assert.IsNotNull(model);
		}

		[TestMethod]
		public void CanSeeOnlyAnnotatedContextProperties()
		{
			var model = GetCtx<Ctx>();

			Assert.AreEqual(2, model.Lists.Count);
			Assert.AreEqual(2, model.Lists["List1"].ContentTypes.Count);
			Assert.AreEqual(1, model.Lists["List2"].ContentTypes.Count);
		}

		[TestMethod]
		public void CanSeeDerivedContextProperties()
		{
			var model = GetCtx<DerivedCtx>();

			Assert.AreEqual(2, model.Lists.Count);
			Assert.AreEqual(2, model.Lists["List1"].ContentTypes.Count);
			Assert.AreEqual(1, model.Lists["List2"].ContentTypes.Count);
		}

		[TestMethod]
		public void ThrowErrorForContextPropertiesWithInvalidType()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() =>
			{
				var model = GetCtx<CtxWithInvalidContextPropertyType>();
			});
		}

		[TestMethod]
		public void ThrowErrorForWriteonlyContextProperties()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() =>
			{
				var model = GetCtx<CtxWithWriteOnlyContextProperty>();
			});
		}

		[TestMethod]
		public void ThrowErrorForWriteOnlyEntityProperty()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() =>
			{
				var model = GetCtx<CtxWithWriteOnlyEntityProperty>();
			});
		}

		private MetaContext GetCtx<T>()
		{
			return new AnnotatedContextMapping<T>().GetMetaContext();
		}
	}
}
