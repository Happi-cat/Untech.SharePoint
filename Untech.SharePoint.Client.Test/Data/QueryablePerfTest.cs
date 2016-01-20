using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Client.Test.Data
{
	[TestClass]
	public class QueryablePerfTest
	{
		[TestMethod]
		public void Measure()
		{
			var ctx = GetContext();
			var tests = new QueryablePerfomance().GetQueryTests();
			var executor = new ClientQueryTestExecutor<NewsModel>
			{
				List = ctx.News,
				SpList = ctx.ClientContext.GetList("News"),
				FilePath = @"C:\Perf-Client.csv"
			};

			tests.Each(executor.Execute);
		}

		private static DataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
	}
}