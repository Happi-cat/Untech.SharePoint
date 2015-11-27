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
			Site = new SPSite(@"http://sp2013", SPUserToken.SystemAccount);
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

			var count = ctx.TestList.Count();
		}

		[TestMethod]
		public void CanAddItem()
		{
			var ctx = new WebDataContext(Web, Bootstrap.GetConfig());
			var generatedTitle = string.Format("Generated Title {0}", DateTime.Now);

			
			Assert.IsTrue(ctx.TestList.Any(n => n.Title != generatedTitle));
			
			ctx.TestList.Add(new TestListItem{ Title = generatedTitle });

			Assert.IsTrue(ctx.TestList.Any(n => n.Title == generatedTitle));
		}

		[TestMethod]
		public void CanDeleteItem()
		{
			var ctx = new WebDataContext(Web, Bootstrap.GetConfig());

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
			var ctx = new WebDataContext(Web, Bootstrap.GetConfig());

			var firstItem = ctx.TestList.First();
			firstItem.Title = "Updated";

			ctx.TestList.Update(firstItem);

			var updatedItem = ctx.TestList.Get(firstItem.Id);

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
