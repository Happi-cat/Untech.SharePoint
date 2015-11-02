using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.MetaModels.Visitors;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Client.Data
{
	public class SpCommonService : ICommonService
	{
		public SpCommonService(ClientContext clientContextweb, Config config)
		{
			ClientContext = clientContextweb;
			Config = config;
		}

		public ClientContext ClientContext { get; private set; }

		public Config Config { get; private set; }

		public IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors
		{
			get
			{
				return new List<IMetaModelVisitor>
				{
					new RuntimeInfoLoader(ClientContext),
					new FieldConverterCreator(Config.FieldConverters),
					new MapperInitializer()
				};
			}
		}
	}
}