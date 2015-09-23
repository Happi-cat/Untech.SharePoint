using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Services;
using Untech.SharePoint.Common.Visitors;

namespace Untech.SharePoint.Common.Data
{
	public abstract class SpContext : ISpContext
	{
		protected SpContext(Config config, ICommonService service)
		{
			Guard.CheckNotNull("config", config);

			Config = config;
			MappingSource = Config.Mappings.Resolve(GetType());
			Model = MappingSource.GetMetaContext();

			service.MetaModelProcessors.Each(n => n.Visit(Model));
		}

		protected Config Config { get; private set; }

		protected IMappingSource MappingSource { get; private set; }

		protected MetaContext Model { get; private set; }

		protected ISpList<TEntity> GetList<TEntity>(Expression<Func<ISpContext, TEntity>> listAccessor)
		{
			var lambdaExpression = (LambdaExpression) listAccessor;

			var memberExp = (MemberExpression) lambdaExpression.Body;
			var listTitle = MappingSource.GetListTitleFromContextMember(memberExp.Member);

			return GetList<TEntity>(Model.Lists[listTitle].ContentTypes[typeof(TEntity)]);
		}

		protected virtual ISpList<TEntity> GetList<TEntity>(MetaContentType contentType)
		{
			throw new NotImplementedException();
		}
	}
}