using System;
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
	public class AnnotatedFieldPartTest 
	{
		[TestMethod]
		public void CanDefineFieldAnnotation()
		{
			var ct = GetContentType<Entity>();

			Assert.AreEqual(2, ct.Fields.Count);
			Assert.AreEqual("OriginalName", ct.Fields["Field1"].InternalName);
			Assert.AreEqual("Field2", ct.Fields["Field2"].InternalName);
		}

		[TestMethod]
		public void CanInheritFieldAnnotation()
		{
			var ct = GetContentType<InheritedAnnotation>();

			Assert.AreEqual(2, ct.Fields.Count);
			Assert.AreEqual("OriginalName", ct.Fields["Field1"].InternalName);
			Assert.AreEqual("Field2", ct.Fields["Field2"].InternalName);
		}

		[TestMethod]
		public void CanOverwriteFieldAnnotation()
		{
			var ct = GetContentType<OverwrittenAnnotation>();

			Assert.AreEqual(2, ct.Fields.Count);
			Assert.AreEqual("NewName", ct.Fields["Field1"].InternalName);
			Assert.AreEqual("Field2", ct.Fields["Field2"].InternalName);
		}

		[TestMethod]
		public void CanRemoveParentField()
		{
			var ct = GetContentType<RemovedField>();

			Assert.AreEqual(1, ct.Fields.Count);
			Assert.AreEqual("OriginalName", ct.Fields["Field1"].InternalName);
		}

		[TestMethod]
		public void CanRemoveThatField()
		{
			var ct = GetContentType<AddedAndRemovedField>();

			Assert.AreEqual(2, ct.Fields.Count);
			Assert.AreEqual("OriginalName", ct.Fields["Field1"].InternalName);
			Assert.AreEqual("Field2", ct.Fields["Field2"].InternalName);
		}

		[TestMethod]
		public void CanIgnoreConstantFields()
		{
			var ct = GetContentType<ConstField>();

			Assert.AreEqual(2, ct.Fields.Count);
			Assert.AreEqual("OriginalName", ct.Fields["Field1"].InternalName);
			Assert.AreEqual("Field2", ct.Fields["Field2"].InternalName);
		}

		[TestMethod]
		public void CanIgnoreStaticProperties()
		{
			var ct = GetContentType<StaticProperty>();

			Assert.AreEqual(2, ct.Fields.Count);
			Assert.AreEqual("OriginalName", ct.Fields["Field1"].InternalName);
			Assert.AreEqual("Field2", ct.Fields["Field2"].InternalName);
		}

		[TestMethod]
		public void ThrowIfFieldIsReadOnly()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() => { GetContentType<ReadOnlyField>(); });
		}

		[TestMethod]
		public void ThrowIfPropertyIsReadOnly()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() => { GetContentType<ReadOnlyProperty>(); });
		}

		[TestMethod]
		public void ThrowIfPropertyIsWriteOnly()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() => { GetContentType<WriteOnlyProperty>(); });
		}

		[TestMethod]
		public void ThrowIfIndexer()
		{
			CustomAssert.Throw<InvalidAnnotationException>(() => { GetContentType<Indexer>(); });
		}

		private MetaContentType GetContentType<T>()
		{
			var metaContext = new AnnotatedContextMapping<Ctx<T>>().GetMetaContext();

			return metaContext.Lists["List"].ContentTypes[typeof (T)];
		}

		#region [Nested Classes]

		public class Ctx<T> : ISpContext
		{
			[SpList]
			public ISpList<T> List { get; set; }

			public Config Config { get; private set; }

			public IMappingSource MappingSource { get; private set; }

			public MetaContext Model { get; private set; }
		}

		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		public class Entity
		{
			[SpField(Name = "OriginalName")]
			public virtual string Field1 { get; set; }

			[SpField]
			public virtual string Field2 { get; set; }

			public string NotAnnotatedField { get; set; }
		}

		public class InheritedAnnotation : Entity
		{
			public override string Field1 { get; set; }
		}

		public class OverwrittenAnnotation : Entity
		{
			[SpField(Name = "NewName")]
			public override string Field1 { get; set; }
		}

		public class RemovedField : Entity
		{
			[SpFieldRemoved]
			public override string Field2 { get; set; }
		}

		public class AddedAndRemovedField : Entity
		{
			[SpField]
			[SpFieldRemoved]
			public virtual string Field3 { get; set; }
		}

		public class ReadOnlyProperty : Entity
		{
			[SpField]
			public string Field3
			{
				get { throw new NotImplementedException(); }
			}
		}

		public class WriteOnlyProperty : Entity
		{
			[SpField]
			public string Field3
			{
				set { throw new NotImplementedException(); }
			}
		}

		public class ReadOnlyField : Entity
		{
			[SpField] public readonly string Field3 = null;
		}

		public class ConstField : Entity
		{
			[SpField] public const string Field3 = null;
		}

		public class StaticProperty: Entity
		{
			[SpField]
			public static string Field3 { get; set; }
		}

		public class Indexer : Entity
		{
			[SpField]
			public string this[string key]
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		}

		#endregion

		
	}
}