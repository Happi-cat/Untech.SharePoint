using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Data
{
	public abstract class SpClientContext<TContext>: SpContext<TContext, SpCommonService>
		where TContext: SpClientContext<TContext>
	{
		protected SpClientContext(ClientContext context, Config config)
			: base(config, null)
		{
			Guard.CheckNotNull("context", context);

			ClientContext = context;
		}

		public ClientContext ClientContext { get; private set; }

		protected override ISpListItemsProvider GetItemsProvider(MetaList list)
		{
			return new SpListItemsProvider(ClientContext, CommonService, list);
		}
	}
}
