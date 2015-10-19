using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Server.Data
{
	public class SpCommonService : ICommonService, IMetaContextProcessor
	{
		public SpCommonService(SPWeb web)
		{
			Web = web;
		}

		public SPWeb Web { get; private set; }

		public IMetaContextProcessor MetaContextProcessor { get { return this; } }
		
		public void Process(MetaContext context)
		{
			throw new System.NotImplementedException();
		}
	}
}