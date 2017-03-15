using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Spec;
using Untech.SharePoint.Common.Spec.Models;

namespace Untech.SharePoint.Server.Data
{
	[TestClass]
	public class BasicOperationsTest
	{
		private static BasicOperationsSpec s_spec;
		private static DataContext s_dataContext;

		[ClassInitialize]
		public static void Init(TestContext ctx)
		{
			s_dataContext = GetContext();
			s_spec = new BasicOperationsSpec("SERVER_BASIC_OPS", s_dataContext);
		}

		[TestMethod]
		public void Spec_AddUpdateDelete()
		{
			s_spec.AddUpdateDelete();
		}

		[TestMethod]
		public void Spec_BatchAddUpdateDelete()
		{
			s_spec.BatchAddUpdateDelete();
		}

		[TestMethod]
		public void Spec_GetAttachments()
		{
			var result = s_dataContext.News.GetAttachments(1).ToList();
		}

		private static DataContext GetContext()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			return new DataContext(new SpServerCommonService(web, Bootstrap.GetConfig()));
		}
	}
}