using Untech.SharePoint.Client.Configuration;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Test.Models
{
	public class Bootstrap
	{
		public Bootstrap()
		{
			Config = ClientConfig.Begin()
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