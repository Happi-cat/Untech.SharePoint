using Microsoft.SharePoint;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Data
{
	public abstract class SpServerContext : SpContext<SpCommonService>
	{
		protected SpServerContext(SPWeb web, Config config) 
			: base(config, new SpCommonService(web))
		{
			Guard.CheckNotNull("web", web);

			Web = web;
		}

		public SPWeb Web { get; private set; }

		protected override ISpListItemsProvider GetItemsProvider(MetaList list)
		{
			return new SpListItemsProvider(Web, CommonService, list);
		}
	}
}