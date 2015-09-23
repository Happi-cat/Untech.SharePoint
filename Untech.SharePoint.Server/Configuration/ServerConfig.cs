using Untech.SharePoint.Common.Configuration;

namespace Untech.SharePoint.Server.Configuration
{
	public static class ServerConfig
	{
		public static ConfigBuilder Begin()
		{
			return (new ConfigBuilder())
				.RegisterConverters(n => n.AddFromAssembly(typeof (ServerConfig).Assembly));
		}
	}
}