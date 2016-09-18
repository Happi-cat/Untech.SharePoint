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
		private static BasicOperationsSpec _spec;
		private static DataContext _dataContext;

		[ClassInitialize]
		public static void Init(TestContext ctx)
		{
			_dataContext = GetContext();
			_spec = new BasicOperationsSpec("CLIENT_BASIC_OPS", _dataContext);
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
			var ctx = new ClientContext(@"http://sp2013dev/sites/orm-test");
			return new DataContext(new SpClientCommonService(ctx, Bootstrap.GetConfig()));
		}
	}
}