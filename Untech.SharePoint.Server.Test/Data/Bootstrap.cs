using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Configuration;

namespace Untech.SharePoint.Server.Test.Data
{
	public class Bootstrap
	{
		public Bootstrap()
		{
			Config = ServerConfig.Begin()
				.RegisterMappings(n => n.Annotated<DataContext>())
				.BuildConfig();
		}

		private Config Config { get; set; }

		public static Config GetConfig()
		{
			return Singleton<Bootstrap>.GetInstance().Config;
		}
	}
}