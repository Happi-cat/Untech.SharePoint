using System;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	public abstract class SpContext<TCommonService> : ISpContext
		where TCommonService: ICommonService
	{
		protected SpContext(Config config, TCommonService commonService)
		{
			Guard.CheckNotNull("config", config);
			Guard.CheckNotNull("commonService", commonService);

			Config = config;
			CommonService = commonService;

			MappingSource = Config.Mappings.Resolve(GetType());
			Model = MappingSource.GetMetaContext();

			CommonService.MetaContextProcessor.Process(Model);
		}

		protected Config Config { get; private set; }

		protected TCommonService CommonService { get; private set; }

		protected IMappingSource MappingSource { get; private set; }

		protected MetaContext Model { get; private set; }

		protected ISpList<TEntity> GetList<TEntity>(Expression<Func<ISpContext, TEntity>> listAccessor)
		{
			var memberExp = (MemberExpression)listAccessor.Body;
			var listTitle = MappingSource.GetListTitleFromContextMember(memberExp.Member);

			return GetList<TEntity>(Model.Lists[listTitle]);
		}

		protected ISpList<TEntity> GetList<TEntity>(MetaList list)
		{
			return new SpList<TEntity>(GetItemsProvider(list));
		}

		protected abstract ISpListItemsProvider GetItemsProvider(MetaList list);
	}
}