using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Spec;
using Untech.SharePoint.Common.Spec.Models;

namespace Untech.SharePoint.Server.Data
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
		public void Spec_Aggregate()
		{
			s_spec.Aggregate();
		}

		[TestMethod]
		public void Spec_Filtering()
		{
			s_spec.Filtering();
		}

		[TestMethod]
		public void Spec_Ordering()
		{
			s_spec.Ordering();
		}

		[TestMethod]
		public void Spec_Paging()
		{
			s_spec.Paging();
		}

		[TestMethod]
		public void Spec_Set()
		{
			s_spec.Set();
		}

		[TestMethod]
		public void Spec_Projection()
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
