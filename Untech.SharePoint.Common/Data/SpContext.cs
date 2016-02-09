using System;
using System.Linq.Expressions;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents base data context.
	/// </summary>
	/// <typeparam name="TContext">Type of the data context.</typeparam>
	[PublicAPI]
	public abstract class SpContext<TContext> : ISpContext
		where TContext : SpContext<TContext>
	{
		private readonly Container<MetaList, ISpListItemsProvider> _listItemsProviders;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpContext{TContext}"/> with the specified config and services.
		/// </summary>
		/// <param name="config">Configuration.</param>
		/// <param name="commonService">Instance of the commen services.</param>
		/// <exception cref="ArgumentNullException"><paramref name="config"/> or <paramref name="commonService"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Cannot find mapping source for current context type in the config,</exception>
		protected SpContext([NotNull]Config config, [NotNull]ICommonService commonService)
		{
			Guard.CheckNotNull("config", config);
			Guard.CheckNotNull("commonService", commonService);

			Config = config;
			CommonService = commonService;

			var contextType = GetType();
			if (!Config.Mappings.CanResolve(contextType))
			{
				throw new InvalidOperationException("Cannot find mapping for this context in Config");
			}

			MappingSource = Config.Mappings.Resolve(contextType);
			Model = MappingSource.GetMetaContext();

			CommonService.MetaModelProcessors.Each(n => n.Visit(Model));

			_listItemsProviders = new Container<MetaList, ISpListItemsProvider>();
		}

		/// <summary>
		/// Gets <see cref="ISpContext.Config"/> that is used by this instance of the <see cref="ISpContext"/>
		/// </summary>
		public Config Config { get; private set; }

		/// <summary>
		/// Gets <see cref="ICommonService"/> instance that used by this data context instance.
		/// </summary>
		[NotNull]
		public ICommonService CommonService { get; private set; }

		/// <summary>
		/// Gets <see cref="IMappingSource"/> that is used by this instance of the <see cref="ISpContext"/>
		/// </summary>
		public IMappingSource MappingSource { get; private set; }

		/// <summary>
		/// Gets <see cref="MetaContext"/> that is used by this instance of the <see cref="ISpContext"/>
		/// </summary>
		public MetaContext Model { get; private set; }

		/// <summary>
		/// Gets <see cref="ISpList{T}"/> instance by list accessor.
		/// </summary>
		/// <typeparam name="TEntity">Type of element.</typeparam>
		/// <param name="listSelector">List property accessor.</param>
		/// <param name="options">List options.</param>
		/// <returns>Instance of the <see cref="ISpList{T}"/>.</returns>
		protected ISpList<TEntity> GetList<TEntity>(Expression<Func<TContext, ISpList<TEntity>>> listSelector, SpListOptions options = SpListOptions.Default)
		{
			var listTitle = GetListTitle(listSelector);

			return GetList<TEntity>(Model.Lists[listTitle], options);
		}

		/// <summary>
		/// Gets list title by list accessor.
		/// </summary>
		/// <typeparam name="TEntity">Type of element.</typeparam>
		/// <param name="listSelector">List property accessor.</param>
		/// <returns>List title</returns>
		protected string GetListTitle<TEntity>(Expression<Func<TContext, ISpList<TEntity>>> listSelector)
		{
			var memberExp = (MemberExpression) listSelector.Body;
			var listTitle = MappingSource.GetListTitleFromContextMember(memberExp.Member);

			if (!Model.Lists.ContainsKey(listTitle))
			{
				throw new InvalidOperationException(string.Format("Can't find meta-list with title '{0}'", listTitle));
			}
			return listTitle;
		}

		/// <summary>
		/// Gets <see cref="ISpList{T}"/> instance for the specified <see cref="MetaList"/>.
		/// </summary>
		/// <typeparam name="TEntity">Type of element.</typeparam>
		/// <param name="list">SP list metadata.</param>
		/// <param name="options">List options.</param>
		/// <returns>Instance of the <see cref="ISpList{T}"/>.</returns>
		protected ISpList<TEntity> GetList<TEntity>(MetaList list, SpListOptions options = SpListOptions.Default)
		{
			var itemsProvider = GetPooledItemsProvider(list);
			
			itemsProvider.FilterByContentType = (options & SpListOptions.NoFilteringByContentType) == 0;

			return new SpList<TEntity>(itemsProvider);
		}

		protected ISpListItemsProvider GetPooledItemsProvider(MetaList list)
		{
			if (!_listItemsProviders.IsRegistered(list))
			{
				_listItemsProviders.Register(list, GetItemsProvider(list));
			}
			return _listItemsProviders.Resolve(list);
		}

		/// <summary>
		/// Gets instance of the <see cref="ISpListItemsProvider"/> for the specified <see cref="MetaList"/>.
		/// </summary>
		/// <param name="list">SP list metadata.</param>
		/// <returns>Instance of the <see cref="ISpListItemsProvider"/>.</returns>
		protected abstract ISpListItemsProvider GetItemsProvider(MetaList list);
	}
}