using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
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
		[ExpectedException(typeof(InvalidAnnotationException))]
		public void ThrowIfFieldIsReadOnly()
		{
			GetContentType<ReadOnlyField>();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidAnnotationException))]
		public void ThrowIfPropertyIsReadOnly()
		{
			GetContentType<ReadOnlyProperty>();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidAnnotationException))]
		public void ThrowIfAutoPropertyIsReadOnly()
		{
			GetContentType<ReadOnlyAutoProperty>();
		}

		[TestMethod]
		public void CanUsePrivateSetter()
		{
			var ct = GetContentType<PrivateSetter>();

			Assert.AreEqual(3, ct.Fields.Count);
			Assert.AreEqual("OriginalName", ct.Fields["Field1"].InternalName);
			Assert.AreEqual("Field2", ct.Fields["Field2"].InternalName);
			Assert.AreEqual("Field3", ct.Fields["Field3"].InternalName);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidAnnotationException))]
		public void ThrowIfPropertyIsWriteOnly()
		{
			GetContentType<WriteOnlyProperty>();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidAnnotationException))]
		public void ThrowIfIndexer()
		{
			GetContentType<Indexer>();
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

			public Config Config { get; }

			public IMappingSource MappingSource { get; }

			public MetaContext Model { get; }
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
			public string Field3 { get; set; }
		}

		public class ReadOnlyProperty : Entity
		{
			[SpField]
			public string Field3
			{
				get { throw new NotImplementedException(); }
			}
		}

		public class PrivateSetter : Entity
		{
			[SpField]
#pragma warning disable RCS1170 // Use read-only auto-implemented property.
			public string Field3 { get; private set; }
#pragma warning restore RCS1170 // Use read-only auto-implemented property.
		}

		public class ReadOnlyAutoProperty : Entity
		{
			[SpField]
			public string Field3 { get; }
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
			[SpField]
			public readonly string Field3 = "Test";
		}

		public class ConstField : Entity
		{
			[SpField]
			public const string Field3 = null;
		}

		public class StaticProperty : Entity
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