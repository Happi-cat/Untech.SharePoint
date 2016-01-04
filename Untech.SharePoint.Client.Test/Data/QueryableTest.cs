using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Client.Test.Data
{
	[TestClass]
	public class QueryableTest
	{
		private static QueryableSpec _spec;

		[ClassInitialize]
		public static void Init(TestContext ctx)
		{
			_spec = new QueryableSpec(GetContext());
			_spec.Init();
		}

		[TestMethod]
		public void Aggregate()
		{
			_spec.Aggregate();
		}

		[TestMethod]
		public void Filtering()
		{
			_spec.Filtering();
		}

		[TestMethod]
		public void Ordering()
		{
			_spec.Ordering();
		}

		[TestMethod]
		public void Paging()
		{
			_spec.Paging();
		}

		[TestMethod]
		public void Set()
		{
			_spec.Set();
		}

		[TestMethod]
		public void Projection()
		{
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
