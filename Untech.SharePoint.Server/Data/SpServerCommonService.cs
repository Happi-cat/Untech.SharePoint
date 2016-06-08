using System.Collections.Generic;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Server.Data.Mapper;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Data
{
	public class SpServerCommonService : ICommonService
	{
		public SpServerCommonService([NotNull] SPWeb web, [NotNull] Config config)
		{
			Guard.CheckNotNull("web", web);
			Guard.CheckNotNull("config", config);

			Web = web;
			Config = config;
		}

		private SPWeb Web { get; }

		public Config Config { get; }

		public IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors => new List<IMetaModelVisitor>
		{
			new RuntimeInfoLoader(Web),
			new MetaFieldWebInitilizer(Web),
			new FieldConverterCreator(Config.FieldConverters),
			new MapperInitializer()
		};

		public ISpListItemsProvider GetItemsProvider([NotNull] MetaList list)
		{
			return new SpListItemsProvider(Web, list);
		}
	}
}