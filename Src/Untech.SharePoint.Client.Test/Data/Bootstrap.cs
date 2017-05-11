using Untech.SharePoint.Configuration;
using Untech.SharePoint.Spec.Models;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Client.Data
{
	public class Bootstrap
	{
		public Bootstrap()
		{
			Config = ClientConfig.Begin()
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