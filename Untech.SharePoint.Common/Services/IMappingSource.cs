using System;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Services
{
	public interface IMappingSource
	{
		Type ContextType { get; }

		IMetaContextProvider ContextProvider { get; }

		IListTitleResolver ListTitleResolver { get; }
	}

	public interface IMappingSource<TContext> : IMappingSource
		where TContext : ISpContext
	{
	}
}