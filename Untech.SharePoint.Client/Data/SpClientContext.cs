using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Client.Data
{
	public class SpClientCommonService : ICommonService
	{
		public IMetaModelVisitor MetaModelProcessor { get; private set; }
	}
	

	public abstract class SpClientContext<TContext>: SpContext<TContext, SpClientCommonService>
		where TContext: SpClientContext<TContext>
	{
		protected SpClientContext(ClientContext context, Config config)
			: base(config, null)
		{

		}
	}
}
