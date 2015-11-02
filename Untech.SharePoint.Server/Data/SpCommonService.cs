using System.Collections.Generic;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Server.Data.Mapper;

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

		public IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors
		{
			get
			{
				return new List<IMetaModelVisitor>
				{
					new RuntimeInfoLoader(Web),
					new FieldConverterCreator(Config.FieldConverters),
					new MapperInitializer()
				};
			}
		}
	}
}