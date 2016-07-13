using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Test.Data
{
	[TestClass]
	public class BasicOperationsTest
	{
		private static BasicOperationsSpec _spec;
		private static DataContext _dataContext;

		[ClassInitialize]
		public static void Init(TestContext ctx)
		{
			_dataContext = GetContext();
			_spec = new BasicOperationsSpec("SERVER_BASIC_OPS", _dataContext);
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

		[TestMethod]
		public void GetAttachments()
		{
			var result = _dataContext.News.GetAttachments(1).ToList();
		}
		private static DataContext GetContext()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			return new DataContext(new SpServerCommonService(web, Bootstrap.GetConfig()));
		}
	}
}