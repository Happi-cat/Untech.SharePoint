using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Server.Data
{
	public class SpCommonService : ICommonService
	{
		public IMetaContextProcessor Processor { get; private set; }
	}

	internal class SpMetaContextProcessor : IMetaContextProcessor
	{
		public SpMetaContextProcessor(SPWeb web)
		{
			Web = web;
		}

		public SPWeb Web { get; private set; }

		public void Process(MetaContext context)
		{
			throw new System.NotImplementedException();
		}
	}
}