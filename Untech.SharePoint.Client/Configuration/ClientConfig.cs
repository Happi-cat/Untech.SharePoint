using Untech.SharePoint.Common.Configuration;

namespace Untech.SharePoint.Client.Configuration
{
	public static class ClientConfig
	{
		public static ConfigBuilder Begin()
		{
			return (new ConfigBuilder())
				.RegisterConverters(n => n.AddFromAssembly(typeof(ConfigBuilder).Assembly))
				.RegisterConverters(n => n.AddFromAssembly(typeof(ClientConfig).Assembly));
		}
	}
}