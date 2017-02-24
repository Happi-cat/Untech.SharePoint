using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Test.Data
{
	[TestClass]
	public class QueryableTest
	{
		private static QueryableSpec s_spec;

		[ClassInitialize]
		public static void Init(TestContext ctx)
		{
			s_spec = new QueryableSpec(GetContext());
			s_spec.Init();
		}

		[TestMethod]
		public void Aggregate()
		{
			s_spec.Aggregate();
		}

		[TestMethod]
		public void Filtering()
		{
			s_spec.Filtering();
		}

		[TestMethod]
		public void Ordering()
		{
			s_spec.Ordering();
		}

		[TestMethod]
		public void Paging()
		{
			s_spec.Paging();
		}

		[TestMethod]
		public void Set()
		{
			s_spec.Set();
		}

		[TestMethod]
		public void Projection()
		{
			s_spec.Projection();
		}

		private static DataContext GetContext()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			return new DataContext(new SpServerCommonService(web, Bootstrap.GetConfig()));
		}
	}
}
