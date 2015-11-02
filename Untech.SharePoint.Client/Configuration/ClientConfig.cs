using Untech.SharePoint.Common.Configuration;

namespace Untech.SharePoint.Client.Configuration
{
	/// <summary>
	/// Provides static method for starting <see cref="ConfigBuilder"/> configuration.
	/// </summary>
	public static class ClientConfig
	{
		/// <summary>
		/// Begins configuration of the <see cref="ConfigBuilder"/> instance.
		/// </summary>
		/// <returns>Config builder with registered built-in field converters from <see cref="Common"/> and <see cref="Client"/> assemblies.</returns>
		public static ConfigBuilder Begin()
		{
			return (new ConfigBuilder())
				.RegisterConverters(n => n.AddFromAssembly(typeof(ConfigBuilder).Assembly))
				.RegisterConverters(n => n.AddFromAssembly(typeof(ClientConfig).Assembly));
		}
	}
}