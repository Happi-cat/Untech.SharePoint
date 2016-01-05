using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Client.Test.Data
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
				FilePath = @"C:\Perf-Client.csv"
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

		[ClassCleanup]
		public static void Cleanup()
		{
			_spec.Dispose();
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
		 
	}
}