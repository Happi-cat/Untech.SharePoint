using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Server.Test.Data
{
	[TestClass]
	public class QueryablePerfTest
	{
		private static QueryableSpec _spec;

		[ClassInitialize]
		public static void Init(TestContext ctx)
		{
			_spec = new QueryableSpec(GetContext())
			{
				FilePath = @"C:\Perf-Server.csv"
			};
			_spec.Init();
		}

		[TestMethod]
		public void Measure()
		{
			_spec.Aggregate();
			_spec.Filtering();
			_spec.Ordering();
			_spec.Paging();
			_spec.Set();
			_spec.Projection();
		}

		private static IDataContext GetContext()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			return new DataContext(web, Bootstrap.GetConfig());
		}
		 
	}
}