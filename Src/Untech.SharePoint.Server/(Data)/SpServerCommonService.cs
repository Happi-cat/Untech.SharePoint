using System.Collections.Generic;
using Microsoft.SharePoint;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Configuration;
using Untech.SharePoint.Converters;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.MetaModels.Visitors;
using Untech.SharePoint.Server.Data;
using Untech.SharePoint.Server.Data.Mapper;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Data
{
	/// <summary>
	/// Represents service that use SSOM (i.e. SharePoint Server Object Model) and can be used inside <see cref="SpContext{TContext}"/>.
	/// </summary>
	public class SpServerCommonService : ICommonService
	{
		private readonly SPWeb _web;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpServerCommonService"/> with the specified instance of the <see cref="SPWeb"/> and defined <see cref="Config"/>.
		/// </summary>
		/// <param name="web">The instance of <see cref="SPWeb"/> that will be used to access SP lists.</param>
		/// <param name="config">The instance of <see cref="Config"/>.</param>
		public SpServerCommonService([NotNull] SPWeb web, [NotNull] Config config)
		{
			Guard.CheckNotNull(nameof(web), web);
			Guard.CheckNotNull(nameof(config), config);

			_web = web;
			Config = config;
		}

		/// <inheritdoc />
		public Config Config { get; }

		/// <inheritdoc />
		public IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors => new List<IMetaModelVisitor>
		{
			new RuntimeInfoLoader(_web),
			new MetaFieldWebInitilizer(_web),
			new FieldConverterCreator(Config.FieldConverters),
			new MapperInitializer()
		};

		/// <inheritdoc />
		public ISpListItemsProvider GetItemsProvider(MetaList list)
		{
			return new SpListItemsProvider(_web, list);
		}
	}
}