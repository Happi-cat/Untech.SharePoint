using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Test.AnnotationMapping.Models;
using TestContext = Untech.SharePoint.Common.Test.AnnotationMapping.Models.TestContext;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	[TestClass]
	public class AnnotatedContextMappingTest
	{	
		[TestMethod]
		public void CanCreateContextAndGetModel()
		{
			var model = GetCtx<TestContext>();

			Assert.IsNotNull(model);
		}

		[TestMethod]
		public void CanSeeOnlyAnnotatedContextProperties()
		{
			var model = GetCtx<TestContext>();

			Assert.AreEqual(2, model.Lists.Count);
			Assert.AreEqual(2, model.Lists["List1"].ContentTypes.Count);
			Assert.AreEqual(1, model.Lists["List2"].ContentTypes.Count);
		}

		[TestMethod]
		public void CanSeeDerivedContextProperties()
		{
			var model = GetCtx<DerivedTestContext>();

			Assert.AreEqual(2, model.Lists.Count);
			Assert.AreEqual(2, model.Lists["List1"].ContentTypes.Count);
			Assert.AreEqual(1, model.Lists["List2"].ContentTypes.Count);
		}

		[TestMethod]
		public void ThrowErrorForContextPropertiesWithInvalidType()
		{
			CustomAssert.Throw<AnnotationException>(() =>
			{
				var model = GetCtx<ContextWithInvalidContextPropertyType>();
			});
		}

		[TestMethod]
		public void ThrowErrorForWriteonlyContextProperties()
		{
			CustomAssert.Throw<AnnotationException>(() =>
			{
				var model = GetCtx<ContextWithWriteOnlyContextProperty>();
			});
		}

		[TestMethod]
		public void ThrowErrorForWriteOnlyEntityProperty()
		{
			CustomAssert.Throw<AnnotationException>(() =>
			{
				var model = GetCtx<ContextWithWriteOnlyEntityProperty>();
			});
		}

		private MetaContext GetCtx<T>()
		{
			return new AnnotatedContextMapping<T>().GetMetaContext();
		}
	}
}
