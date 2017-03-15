using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Common.Spec;
using Untech.SharePoint.Common.Spec.Models;
using Untech.SharePoint.Common.TestTools.QueryTests;

namespace Untech.SharePoint.Client.Data
{
	[TestClass]
	public class QueryablePerfTest
	{
		[TestMethod]
		[TestCategory("Performance")]
		public void Performance_Measure()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			var ctx = GetContext(context);
			var queries = new QueryablePerfomance().GetQueries();
			var executor = new ClientTestQueryExecutor<NewsModel>(ctx.Model.Lists["Lists/News"])
			{
				List = ctx.News,
				SpList = context.GetListByUrl("/Lists/News"),
				FilePath = @"C:\Perf-Client.csv"
			};

			foreach (var query in queries)
			{
				((TestQueryBuilder<NewsModel>)query).Accept(executor);
			}
		}

		private static DataContext GetContext(ClientContext context)
		{
			return new DataContext(new SpClientCommonService(context, Bootstrap.GetConfig()));
		}
	}
}