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
	public class AnnotatedContentTypeMappingTest
	{
		[TestMethod]
		public void CanOmitContentTypeAnnotation()
		{
			var ct = GetContentType<Entity>();
			
			Assert.IsTrue(string.IsNullOrEmpty(ct.Id));
		}


		[TestMethod]
		public void CanDefineContentTypeAnnotation()
		{
			var ct = GetContentType<Item>();

			Assert.AreEqual("0x01", ct.Id);
		}

		[TestMethod]
		public void CanOverwriteContentTypeAnnotation()
		{
			var ct = GetContentType<ChildItem1>();

			Assert.AreEqual("0x0101", ct.Id);
		}

		[TestMethod]
		public void CanInheritContentTypeAnnotation()
		{
			var ct = GetContentType<ChildItem2>();

			Assert.AreEqual("0x01", ct.Id);
		}

		private MetaContentType GetContentType<T>()
		{
			var metaContext = new AnnotatedContextMapping<Ctx<T>>().GetMetaContext();

			return metaContext.Lists["List"].ContentTypes[typeof(T)];
		}

		#region [Nested Classes]

		public class Ctx<T> : ISpContext
		{
			[SpList("List")]
			public ISpList<T> List { get; set; }

			public Config Config { get; private set; }

			public IMappingSource MappingSource { get; private set; }

			public MetaContext Model { get; private set; }
		}

		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		public class Entity
		{
			[SpField]
			public string Field1 { get; set; }

			[SpField]
			public string Field2 { get; set; }
		}

		[SpContentType(Id = "0x01")]
		public class Item : Entity
		{

		}

		[SpContentType(Id = "0x0101")]
		public class ChildItem1 : Item
		{

		}

		public class ChildItem2 : Item
		{

		}

		#endregion
	}
}