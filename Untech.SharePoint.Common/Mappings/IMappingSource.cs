using System;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Mappings
{
	/// <summary>
	/// Represents interface that can create <see cref="MetaContext"/> and resolve list title for the specified member of this context.
	/// </summary>
	public interface IMappingSource : IMetaContextProvider, IListTitleResolver
	{
		/// <summary>
		/// Gets <see cref="Type"/> of the associated Data Context class.
		/// </summary>
		Type ContextType { get; }
	}

	/// <summary>
	/// Represents interface that can create <see cref="MetaContext"/> and resolve list title for the specified member of this context.
	/// </summary>
	/// <typeparam name="TContext">Type of the data context that is associated with this instance of the <see cref="IMappingSource{TContext}"/></typeparam>
	public interface IMappingSource<TContext> : IMappingSource
		where TContext : ISpContext
	{
	}
}