using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Data;
using Untech.SharePoint.Spec;
using Untech.SharePoint.Spec.Models;
using Untech.SharePoint.TestTools.QueryTests;

namespace Untech.SharePoint.Server.Data
{
	[TestClass]

	public class QueryablePerfTest
	{
		[TestMethod]
		[TestCategory("Performance")]
		public void Performance_Measure()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			var ctx = GetContext(web);
			var queries = new QueryablePerfomance().GetQueries();
			var executor = new ServerTestQueryExecutor<NewsModel>(ctx.Model.Lists["Lists/News"])
			{
				List = ctx.News,
				SpList = web.GetList(web.ServerRelativeUrl + "/Lists/News"),
				FilePath = @"C:\Perf-Server.csv"
			};

			foreach (var query in queries)
			{
				((TestQueryBuilder<NewsModel>)query).Accept(executor);
			}
		}

		private static DataContext GetContext(SPWeb web)
		{
			return new DataContext(new SpServerCommonService(web, Bootstrap.GetConfig()));
		}
	}
}