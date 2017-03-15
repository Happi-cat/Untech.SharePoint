using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Spec.Models;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Configuration;

namespace Untech.SharePoint.Server.Data
{
	public class Bootstrap
	{
		public Bootstrap()
		{
			Config = ServerConfig.Begin()
				.RegisterMappings(n => n.Annotated<DataContext>())
				.BuildConfig();
		}

		private Config Config { get; }

		public static Config GetConfig()
		{
			return Singleton<Bootstrap>.GetInstance().Config;
		}
	}
}