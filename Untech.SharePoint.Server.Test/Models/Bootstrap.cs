using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Configuration;

namespace Untech.SharePoint.Server.Test.Models
{
	public class Bootstrap
	{
		public Bootstrap()
		{
			Config = ServerConfig.Begin()
				.RegisterMappings(n => n.Annotated<WebDataContext>())
				.BuildConfig();
		}

		private Config Config { get; set; }

		public static Config GetConfig()
		{
			return Singleton<Bootstrap>.GetInstance().Config;
		}
	}
}