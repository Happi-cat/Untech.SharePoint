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
			_spec = new QueryableSpec(GetContext());
			_spec.Init();
		}

		[TestMethod]
		public void Measure()
		{
			_spec.MeasurePerfomance(@"C:\Perf-Client.csv");
		}

		private static IDataContext GetContext()
		{
			var context = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(context, Bootstrap.GetConfig());
		}
		 
	}
}