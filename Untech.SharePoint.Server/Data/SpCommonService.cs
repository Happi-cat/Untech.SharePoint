using System.Collections.Generic;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Server.MetaModels.Visitors;

namespace Untech.SharePoint.Server.Data
{
	public class SpCommonService : ICommonService
	{
		public SpCommonService(SPWeb web, Config config)
		{
			Web = web;
			Config = config;
		}

		public SPWeb Web { get; private set; }

		public Config Config { get; private set; }

		public IMetaModelVisitor MetaModelProcessor
		{
			get
			{
				return new MetaModelProcessor(new List<IMetaModelVisitor>
				{
					new RuntimeInfoLoader(Web),
					new FieldConverterInitializer(Config.FieldConverters),
					new MapperInitializer()
				});
			}
		}
	}
}