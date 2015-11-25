using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Test.Models;

namespace Untech.SharePoint.Client.Test
{
	[TestClass]
	public class ClientContextTest : IDisposable
	{
		public ClientContextTest()
		{
			Context = new ClientContext("http://sp2013");
		}

		private ClientContext Context { get; set; }

		[TestMethod]
		public void CanRun()
		{
			var ctx = new WebDataContext(Context, Bootstrap.GetConfig());
		}

		[TestMethod]
		public void CanCount()
		{
			var ctx = new WebDataContext(Context, Bootstrap.GetConfig());

			var count = ctx.TestList.Count();
		}

		[TestMethod]
		public void CanAddItem()
		{
			var ctx = new WebDataContext(Context, Bootstrap.GetConfig());
			var generatedTitle = string.Format("Generated Title {0}", DateTime.Now);

			
			Assert.IsTrue(ctx.TestList.Any(n => n.Title != generatedTitle));
			
			ctx.TestList.Add(new TestListItem{ Title = generatedTitle });

			Assert.IsTrue(ctx.TestList.Any(n => n.Title == generatedTitle));
		}

		[TestMethod]
		public void CanDeleteItem()
		{
			var ctx = new WebDataContext(Context, Bootstrap.GetConfig());

			var countBefore = ctx.TestList.Count();
			var firstItem = ctx.TestList.First();

			ctx.TestList.Delete(firstItem);

			Assert.IsTrue(ctx.TestList.Any(n => n.Id != firstItem.Id));

			var countAfter = ctx.TestList.Count();

			Assert.AreEqual(countBefore, countAfter + 1);
		}

		[TestMethod]
		public void CanUpdateItem()
		{
			var ctx = new WebDataContext(Context, Bootstrap.GetConfig());

			var firstItem = ctx.TestList.First();
			firstItem.Title = "Updated";

			ctx.TestList.Update(firstItem);

			var updatedItem = ctx.TestList.Get(firstItem.Id);

			Assert.AreEqual(updatedItem.Title, "Updated");
			Assert.AreNotEqual(firstItem.Modified, updatedItem.Modified);
		}

		public void Dispose()
		{
			if (Context != null)
			{
				Context.Dispose();
			}
		}
	}
}
