using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.Mapper;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Client.Data
{
	internal class SpCommonService : ICommonService
	{
		public SpCommonService(ClientContext clientContextweb, Config config)
		{
			ClientContext = clientContextweb;
			Config = config;
		}

		private ClientContext ClientContext { get; set; }

		private Config Config { get; set; }

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