using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Client.Data
{
	public abstract class SpClientContext: SpContext
	{
		protected SpClientContext(ClientContext context, Config config)
			: base(config, null)
		{

		}
	}
}
