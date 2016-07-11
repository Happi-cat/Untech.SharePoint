using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.QueryTests;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Test.Data
{
	[TestClass]
	public class QueryablePerfTest
	{
		[TestMethod]
		public void Measure()
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