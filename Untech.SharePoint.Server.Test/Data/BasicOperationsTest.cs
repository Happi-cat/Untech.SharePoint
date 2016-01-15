using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec;

namespace Untech.SharePoint.Server.Test.Data
{
	[TestClass]
	public class BasicOperationsTest
	{
		private static BasicOperationsSpec _spec;

		[ClassInitialize]
		public static void Init(TestContext ctx)
		{
			_spec = new BasicOperationsSpec("SERVER_BASIC_OPS", GetContext());
		}

		[TestMethod]
		public void AddUpdateDelete()
		{
			_spec.AddUpdateDelete();
		}

		[TestMethod]
		public void BatchAddUpdateDelete()
		{
			_spec.BatchAddUpdateDelete();
		}

		private static DataContext GetContext()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			return new DataContext(web, Bootstrap.GetConfig());
		}
	}
}