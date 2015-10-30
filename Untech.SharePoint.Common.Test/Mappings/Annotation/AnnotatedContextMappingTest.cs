using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation
{
	[TestClass]
	public class AnnotatedContextMappingTest
	{
		[TestMethod]
		public void CanCreateContextAndGetModel()
		{
			var model = GetCtx<Ctx>();

			Assert.AreEqual(1, model.Lists.Count);
			Assert.AreEqual("List1", model.Lists["List1"].Title);
		}

		[TestMethod]
		public void CanInheritListAnnotation()
		{
			var model = GetCtx<InheritedAnnotationCtx>();

			Assert.AreEqual(1, model.Lists.Count);
			Assert.AreEqual("List1", model.Lists["List1"].Title);
		}

		[TestMethod]
		public void CanOverwriteListAnnotation()
		{
			var model = GetCtx<OverwrittenAnnotationCtx>();

			Assert.AreEqual(1, model.Lists.Count);
			Assert.AreEqual("ListTitle", model.Lists["ListTitle"].Title);
		}

		[TestMethod]
		public void CanSeeMultipleContentTypes()
		{
			var model = GetCtx<MultipleContentTypesCtx>();

			Assert.AreEqual(1, model.Lists.Count);
			Assert.AreEqual(2, model.Lists["Title"].ContentTypes.Count);
		}

		[TestMethod]
		public void CanSeeIndexer()
		{
			var model = GetCtx<IndexerPropertyCtx>();

			Assert.AreEqual(2, model.Lists.Count);
		}


		[TestMethod]
		public void ThrowIfListPropertyTypeIsInvalid()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() => { GetCtx<InvalidPropertyTypeCtx>(); });
		}

		[TestMethod]
		public void ThrowIfListPropertyIsWriteOnly()
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
			[SpList]
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
			[SpList(Title = "ListTitle")]
			public override ISpList<Entity> List1 { get; set; }
		}

		public class InvalidPropertyTypeCtx : Ctx
		{
			[SpList]
			public string List2 { get; set; }
		}

		public class WriteOnlyPropertyCtx : Ctx
		{
			[SpList]
			public ISpList<Entity> List2
			{
				set { throw new NotImplementedException(); }
			}
		}

		public class IndexerPropertyCtx : Ctx
		{
			[SpList]
			public ISpList<Entity> this[string title]
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		}

		public class MultipleContentTypesCtx : Ctx
		{
			[SpList(Title = "Title")]
			public override ISpList<Entity> List1 { get; set; }

			[SpList(Title = "Title")]
			public ISpList<ChildEntity> List2 { get; set; }
		}

		public class Entity
		{
			[SpField(Name = "OriginalName")]
			public virtual string Field1 { get; set; }

			[SpField]
			public virtual string Field2 { get; set; }
		}

		public class ChildEntity : Entity
		{
			
		}

		#endregion

	}
}
