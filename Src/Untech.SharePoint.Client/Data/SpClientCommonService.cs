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
	/// <summary>
	/// Represents service that use CSOM (i.e. SharePoint Client Object Model) and can be used inside <see cref="SpContext{TContext}"/>.
	/// </summary>
	public class SpClientCommonService : ICommonService
	{
		private readonly ClientContext _clientContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpClientCommonService"/> with the specified instance of the <see cref="_clientContext"/> and defined <see cref="Config"/>.
		/// </summary>
		/// <param name="clientContext">The instance of <see cref="ClientContext"/> that will be used to access SP lists.</param>
		/// <param name="config">The instance of <see cref="Config"/>.</param>
		public SpClientCommonService([NotNull]ClientContext clientContext, [NotNull]Config config)
		{
			Guard.CheckNotNull(nameof(clientContext), clientContext);
			Guard.CheckNotNull(nameof(config), config);

			_clientContext = clientContext;
			Config = config;
		}

		/// <inheritdoc />
		public Config Config { get; }

		/// <inheritdoc />
		public IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors => new List<IMetaModelVisitor>
		{
			new RuntimeInfoLoader(_clientContext),
			new FieldConverterCreator(Config.FieldConverters),
			new MapperInitializer()
		};

		/// <inheritdoc />
		public ISpListItemsProvider GetItemsProvider([NotNull] MetaList list)
		{
			return new SpListItemsProvider(_clientContext, list);
		}
	}
}