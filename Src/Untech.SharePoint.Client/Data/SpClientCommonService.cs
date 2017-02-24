using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.Mapper;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Data
{
	public class SpClientCommonService : ICommonService
	{
		public SpClientCommonService([NotNull]ClientContext clientContext, [NotNull]Config config)
		{
			Guard.CheckNotNull(nameof(clientContext), clientContext);
			Guard.CheckNotNull(nameof(config), config);

			ClientContext = clientContext;
			Config = config;
		}

		private ClientContext ClientContext { get; }

		/// <inheritdoc />
		public Config Config { get; }

		/// <inheritdoc />
		public IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors => new List<IMetaModelVisitor>
		{
			new RuntimeInfoLoader(ClientContext),
			new FieldConverterCreator(Config.FieldConverters),
			new MapperInitializer()
		};

		/// <inheritdoc />
		public ISpListItemsProvider GetItemsProvider([NotNull] MetaList list)
		{
			return new SpListItemsProvider(ClientContext, list);
		}
	}
}