using System;
using System.Reflection;
using Untech.SharePoint.Data;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.MetaModels.Providers;

namespace Untech.SharePoint.Mappings.ClassLike
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