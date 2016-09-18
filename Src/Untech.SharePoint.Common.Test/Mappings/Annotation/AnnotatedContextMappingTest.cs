using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
	public class AnnotatedContextMappingTest
	{
		[TestMethod]
		public void CanCreateContextAndGetModel()
		{
			var model = GetCtx<Ctx>();

			Assert.AreEqual(1, model.Lists.Count);
			Assert.AreEqual("List1", model.Lists["List1"].Url);
		}

		[TestMethod]
		public void CanInheritListAnnotation()
		{
			var model = GetCtx<InheritedAnnotationCtx>();

			Assert.AreEqual(1, model.Lists.Count);
			Assert.AreEqual("List1", model.Lists["List1"].Url);
		}

		[TestMethod]
		public void CanOverwriteListAnnotation()
		{
			var model = GetCtx<OverwrittenAnnotationCtx>();

			Assert.AreEqual(1, model.Lists.Count);
			Assert.AreEqual("ListTitle", model.Lists["ListTitle"].Url);
		}

		[TestMethod]
		public void CanHaveMultipleContentTypes()
		{
			var model = GetCtx<MultipleContentTypesCtx>();

			Assert.AreEqual(1, model.Lists.Count);
			Assert.AreEqual(2, model.Lists["Url"].ContentTypes.Count);
		}

		[TestMethod]
		public void ThrowIfIndexer()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() => { GetCtx<IndexerPropertyCtx>(); });
		}


		[TestMethod]
		public void ThrowIfTypeIsInvalid()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() => { GetCtx<InvalidPropertyTypeCtx>(); });
		}

		[TestMethod]
		public void ThrowIfPropertyIsWriteOnly()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() => { GetCtx<WriteOnlyPropertyCtx>(); });
		}

		private MetaContext GetCtx<T>()
		{
			return new AnnotatedContextMapping<T>().GetMetaContext();
		}

		#region [Nested Classes]

		public class Ctx : ISpContext
		{
			[SpList("List1")]
			public virtual ISpList<Entity> List1 { get; set; }

			public Config Config { get; private set; }

			public IMappingSource MappingSource { get; private set; }

			public MetaContext Model { get; private set; }
		}

		public class InheritedAnnotationCtx : Ctx
		{
			public override ISpList<Entity> List1 { get; set; }
		}

		public class OverwrittenAnnotationCtx : Ctx
		{
			[SpList("ListTitle")]
			public override ISpList<Entity> List1 { get; set; }
		}

		public class InvalidPropertyTypeCtx : Ctx
		{
			[SpList("List2")]
			public string List2 { get; set; }
		}

		public class WriteOnlyPropertyCtx : Ctx
		{
			[SpList("List2")]
			public ISpList<Entity> List2
			{
				set { throw new NotImplementedException(); }
			}
		}

		public class IndexerPropertyCtx : Ctx
		{
			[SpList("List")]
			public ISpList<Entity> this[string title]
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		}

		public class MultipleContentTypesCtx : Ctx
		{
			[SpList("Url")]
			public override ISpList<Entity> List1 { get; set; }

			[SpList("Url")]
			public ISpList<ChildEntity> List2 { get; set; }
		}

		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		public class Entity
		{
			[SpField(Name = "OriginalName")]
			public string Field1 { get; set; }

			[SpField]
			public string Field2 { get; set; }
		}

		public class ChildEntity : Entity
		{
			
		}

		#endregion

	}
}
