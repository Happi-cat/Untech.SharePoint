using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Server.Test.Models;

namespace Untech.SharePoint.Server.Test
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public class ServerContextTest : IDisposable
	{
		public ServerContextTest()
		{
			Site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			Web = Site.OpenWeb();
		}

		private SPSite Site { get; set; }
		private SPWeb Web { get; set; }

		[TestMethod]
		public void CanRun()
		{
			var ctx = new WebDataContext(Web, Bootstrap.GetConfig());
		}

		[TestMethod]
		public void CanCount()
		{
			var ctx = new WebDataContext(Web, Bootstrap.GetConfig());

			var count = ctx.News.Count();
		}

		[TestMethod]
		public void CanAddItem()
		{
			var ctx = new WebDataContext(Web, Bootstrap.GetConfig());
			var generatedTitle = string.Format("Generated Title {0}", DateTime.Now);

			
			Assert.IsTrue(ctx.News.Any(n => n.Title != generatedTitle));
			
			ctx.News.Add(new NewsItem{ Title = generatedTitle });

			Assert.IsTrue(ctx.News.Any(n => n.Title == generatedTitle));
		}

		[TestMethod]
		public void CanDeleteItem()
		{
			var ctx = new WebDataContext(Web, Bootstrap.GetConfig());

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
			var ctx = new WebDataContext(Web, Bootstrap.GetConfig());

			var firstItem = ctx.News.First();
			firstItem.Title = "Updated";

			ctx.News.Update(firstItem);

			var updatedItem = ctx.News.Get(firstItem.Id);

			Assert.AreEqual(updatedItem.Title, "Updated");
			Assert.AreNotEqual(firstItem.Modified, updatedItem.Modified);
		}

		public void Dispose()
		{
			if (Web != null)
			{
				Web.Dispose();
				Web = null;
			}
			if (Site != null)
			{
				Site.Dispose();
				Site = null;
			}
		}
	}
}
