using System.IO;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Server.Test.Data
{
	[TestClass]
	public class QueryablePerfTest
	{
		[TestMethod]
		public void Measure()
		{
			var ctx = GetContext();
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			var tests = new QueryablePerfomance().GetQueryTests();
			var executor = new ServerQueryTestExecutor<NewsModel>
			{
				List = ctx.News,
				SpList = web.Lists["News"],
				FilePath = @"C:\Perf-Server.csv"
			};

			tests.Each(executor.Execute);
		}

		private static IDataContext GetContext()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			return new DataContext(web, Bootstrap.GetConfig());
		}
	}
}