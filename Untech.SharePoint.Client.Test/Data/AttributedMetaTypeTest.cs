using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.AttributedMapping;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Client.Meta;

namespace Untech.SharePoint.Client.Test.Data
{
	[TestClass]
	public class AttributedMetaTypeTest
	{
		internal class FakeMetaList : MetaList
		{
			public FakeMetaList()
				: base(new AttributedMetaDataMemberTest.FakeMetaModel())
			{
			}

			public override string ListTitle
			{
				get { throw new NotImplementedException(); }
			}

			public override SpFieldCollection Fields
			{
				get { throw new NotImplementedException(); }
			}

			public override MetaType ItemType
			{
				get { throw new NotImplementedException(); }
			}
		}

		internal class FakeMetaModel : MetaModel
		{
			public override MetaList GetList(string listTitle, Type itemType)
			{
				throw new NotImplementedException();
			}

			public override IEnumerable<MetaList> GetLists()
			{
				throw new NotImplementedException();
			}

			public override MetaList GetList(System.Reflection.MemberInfo memberInfo)
			{
				throw new NotImplementedException();
			}
		}

		internal class TestModel
		{
			[SpField]
			public string Property1 { get; set; }

			[SpField(InternalName = "Custom")]
			public string Property2 { get; set; }

			public string NonSpField { get; set; }
		}

		[TestMethod]
		public void CanCreate()
		{
			var type = new AttributedMetaType(new FakeMetaModel(), new FakeMetaList(), typeof(TestModel));

			Assert.AreEqual(2, type.DataMembers.Count);
			Assert.AreEqual("Property1", type.DataMembers.GetMemberByName("Property1").SpFieldInternalName);
			Assert.AreEqual("Custom", type.DataMembers.GetMemberByName("Property2").SpFieldInternalName);
		}
	}
}
