using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Test.Models;

namespace Untech.SharePoint.Client.Test
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public class ClientContextTest : IDisposable
	{
		public ClientContextTest()
		{
			Context = new ClientContext("http://sp2013dev/sites/orm-test");
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

			var count = ctx.News.Count();
		}

		[TestMethod]
		public void CanAddItem()
		{
			var ctx = new WebDataContext(Context, Bootstrap.GetConfig());
			var generatedTitle = string.Format("Generated Title {0}", DateTime.Now);

			
			Assert.IsTrue(ctx.News.Any(n => n.Title != generatedTitle));
			
			var newItem = ctx.News.Add(new NewsItem{ Title = generatedTitle });

			Assert.IsTrue(ctx.News.Any(n => n.Title == generatedTitle));
		}

		[TestMethod]
		public void CanDeleteItem()
		{
			var ctx = new WebDataContext(Context, Bootstrap.GetConfig());

			var countBefore = ctx.News.Count();
			var firstItem = ctx.News.First();

			ctx.News.Delete(firstItem);

			Assert.IsTrue(ctx.News.Any(n => n.Id != firstItem.Id));

			var countAfter = ctx.News.Count();

			Assert.AreEqual(countBefore, countAfter + 1);
		}

		[TestMethod]
		public void CanUpdateItem()
		{
			var ctx = new WebDataContext(Context, Bootstrap.GetConfig());

			var firstItem = ctx.News.First();
			firstItem.Title = "Updated";

			ctx.News.Update(firstItem);

			var updatedItem = ctx.News.Get(firstItem.Id);

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
