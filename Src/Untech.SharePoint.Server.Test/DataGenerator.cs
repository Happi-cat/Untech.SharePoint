using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Spec.Models;
using Untech.SharePoint.Common.TestTools.DataGenerators;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server
{
	public class DataGenerator
	{
		public static void Generate()
		{
			var site = new SPSite(@"http://sp2013dev/sites/orm-test", SPUserToken.SystemAccount);
			var web = site.OpenWeb();
			var context = new DataContext(new SpServerCommonService(web, Bootstrap.GetConfig()));
			var users = GetUsers(web.SiteUsers);

			(new TestDataGenerator(context, users)).Generate();
		}

		private static List<UserInfo> GetUsers(SPUserCollection users)
		{
			return users
				.Cast<SPUser>()
				.Select(n => new UserInfo { Id = n.ID })
				.ToList();
		}
	}
}
