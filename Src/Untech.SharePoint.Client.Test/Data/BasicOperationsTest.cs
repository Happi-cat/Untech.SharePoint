using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Common.Test.Spec;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Client.Test.Data
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
			s_spec = new BasicOperationsSpec("CLIENT_BASIC_OPS", s_dataContext);
		}

		[TestMethod]
		public void AddUpdateDelete()
		{
			s_spec.AddUpdateDelete();
		}

		[TestMethod]
		public void BatchAddUpdateDelete()
		{
			s_spec.BatchAddUpdateDelete();
		}

		[TestMethod]
		public void GetAttachments()
		{
			var result = s_dataContext.News.GetAttachments(1).ToList();
		}

		private static DataContext GetContext()
		{
			var ctx = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(new SpClientCommonService(ctx, Bootstrap.GetConfig()));
		}
	}
}