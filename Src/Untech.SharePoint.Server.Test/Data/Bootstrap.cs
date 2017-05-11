using Untech.SharePoint.Configuration;
using Untech.SharePoint.Spec.Models;
using Untech.SharePoint.Utils;

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