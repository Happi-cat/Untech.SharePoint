using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Client.Data
{
	public class SpClientCommonService : ICommonService
	{
		public IMetaContextProcessor Processor { get; private set; }
	}
	

	public abstract class SpClientContext: SpContext<SpClientCommonService>
	{
		protected SpClientContext(ClientContext context, Config config)
			: base(config, null)
		{

		}
	}
}
