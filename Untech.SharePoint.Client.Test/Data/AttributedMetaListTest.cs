using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.AttributedMapping;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Client.Meta;

namespace Untech.SharePoint.Client.Test.Data
{
	[TestClass]
	public class AttributedMetaListTest
	{
		internal class FakeMetaModel : MetaModel, ISpFieldsResolver
		{
			public override MetaList GetList(string listTitle, Type itemType)
			{
				throw new NotImplementedException();
			}

			public override IEnumerable<MetaList> GetLists()
			{
				throw new NotImplementedException();
			}

			public SpFieldCollection GetFields(string listTitle)
			{
				return new SpFieldCollection(new List<Field>());
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

			public string NonSpField { get; set; }
		}

		[TestMethod]
		public void CanCreate()
		{
			var list = new AttributedMetaList(new FakeMetaModel(), new SpListAttribute("List"), typeof(TestModel), new FakeMetaModel());

			Assert.AreEqual("List", list.ListTitle);
		}
	}
}
