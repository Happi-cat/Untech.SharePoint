using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Data;

namespace Untech.SharePoint.Client.Test.Data
{
	[TestClass]
	public class AttributedMetaDataMemberTest
	{
		internal class FakeMetaList : MetaList
		{
			public FakeMetaList() : base(new FakeMetaModel())
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
		}

		internal class FakeMetaType : MetaType
		{
			public FakeMetaType(Type type)
				: base(new FakeMetaModel(), new FakeMetaList(), type)
			{
			}

			public  override DataMemberCollection DataMembers
			{
				get { throw new NotImplementedException(); }
			}
		}

		internal class TestModel
		{
			[SpField]
			public string Property1 { get; set; }

		}

		[TestMethod]
		public void CanCreate()
		{
			var member = new AttributedMetaDataMember(new FakeMetaType(typeof(TestModel)), typeof(TestModel).GetProperty("Property1"));
		}
	}
}