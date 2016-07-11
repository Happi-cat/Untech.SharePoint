using System;
using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Mappings.ClassLike
{
	internal class ClassLikeMappingSource<TContext> : MappingSource<TContext>
		where TContext : ISpContext
	{
		private readonly IListUrlResolver _listUrlResolver;
		private readonly IMetaContextProvider _metaContextProvider;

		public ClassLikeMappingSource(ContextMap<TContext> contextMap)
		{
			_listUrlResolver = contextMap;
			_metaContextProvider = contextMap;
		}

		public override MetaContext GetMetaContext()
		{
			return _metaContextProvider.GetMetaContext();
		}

		public override string GetListUrlFromContextMember(MemberInfo member)
		{
			return _listUrlResolver.GetListUrlFromContextMember(member);
		}
	}
}