using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Test.AnnotationMapping.Models;
using TestContext = Untech.SharePoint.Common.Test.AnnotationMapping.Models.TestContext;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	[TestClass]
	public class AnnotatedFieldPartTest 
	{
		[TestMethod]
		public void CanInheritFieldAnnotation()
		{
			var model = GetCtx<TestContext>();

			Assert.AreEqual("OldInternalName", GetField<DerivedTestEntityWithIheritedAnnotation>(model, "List1", "OverrideProperty").InternalName);
		}

		[TestMethod]
		public void CanOverwriteFieldAnnotation()
		{
			var model = new AnnotatedContextMapping<TestContext>().GetMetaContext();

			Assert.AreEqual("NewInternalName", GetField<DerivedTestEntityWithOverwrittenAnnotation>(model, "List2", "OverrideProperty").InternalName);
		}

		[TestMethod]
		public void ThrowErrorForReadOnlyEntityField()
		{
			CustomAssert.Throw<AnnotationException>(() =>
			{
				var model = GetCtx<ContextWithReadOnlyEntityField>();
			});
		}

		[TestMethod]
		public void ThrowErrorForEntityIndexer()
		{
			CustomAssert.Throw<AnnotationException>(() =>
			{
				var model = GetCtx<ContextWithEntityIndexer>();
			});
		}

		[TestMethod]
		public void ThrowErrorForReadOnlyEntityProperty()
		{
			CustomAssert.Throw<AnnotationException>(() =>
			{
				var model = GetCtx<ContextWithReadOnlyEntityProperty>();
			});
		}

		private MetaContext GetCtx<T>()
		{
			return new AnnotatedContextMapping<T>().GetMetaContext();
		}

		private MetaField GetField<T>(MetaContext context, string list, string field)
		{
			return context.Lists[list].ContentTypes[typeof(T)].Fields[field];
		}
	}
}