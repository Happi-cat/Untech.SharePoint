using System;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Mappings
{
	public interface IMappingSource : IMetaContextProvider, IListTitleResolver
	{
		Type ContextType { get; }
	}

	public interface IMappingSource<TContext> : IMappingSource
		where TContext : ISpContext
	{
	}
}