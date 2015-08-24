using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Data;

namespace Untech.SharePoint.Client.Test.Data
{
	[TestClass]
	public class AttributedMetaModelTest
	{
		internal class FakeSpFieldsResolver : ISpFieldsResolver
		{
			public SpFieldCollection GetFields(string listTitle)
			{
				return new SpFieldCollection(new List<Field>());
			}
		}

		internal class TestDataContext : SpDataContext<TestDataContext>
		{
			[SpList("List1")]
			public SpList<TestModel> ListInstance1 { get { return GetList(n => n.ListInstance1); } }
			
			[SpList("List2")]
			public SpList<TestModel> ListInstance2 { get { return GetList(n => n.ListInstance2); } }

			public TestDataContext(ClientContext context) : base(context)
			{
			}
		}

		internal class TestModel
		{
			[SpField]
			public string Property1 { get; set; }

			[SpField]
			public string Property2 { get; set; }
		}

		[TestMethod]
		public void CanCreate()
		{
			var model  = new AttributedMetaModel(typeof(TestDataContext), new FakeSpFieldsResolver());

			Assert.AreEqual(2, model.GetLists().Count());
			Assert.IsTrue(model.GetLists().Any(n=> n.ListTitle == "List1"));
			Assert.IsTrue(model.GetLists().Any(n => n.ListTitle == "List2"));
		}
	}
}
