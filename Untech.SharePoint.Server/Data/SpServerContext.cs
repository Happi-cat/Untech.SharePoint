using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Server.Data
{
	public abstract class SpServerContext : SpContext
	{
		protected SpServerContext(Config config) 
			: base(config, null)
		{

		}
	}
}