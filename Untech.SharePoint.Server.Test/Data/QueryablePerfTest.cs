using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Test.Data
{
	[TestClass]
	public class QueryablePerfTest
	{
		//[TestMethod]
		public void Measure()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			var ctx = GetContext(web);
			var tests = new QueryablePerfomance().GetQueryTests();
			var executor = new ServerQueryTestExecutor<NewsModel>
			{
				List = ctx.News,
				SpList = web.Lists["News"],
				FilePath = @"C:\Perf-Server.csv"
			};

			tests.Each(executor.Execute);
		}

		private static DataContext GetContext(SPWeb web)
		{
			return new DataContext(new SpServerCommonService(web, Bootstrap.GetConfig()));
		}
	}
}