using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Mappings
{
	/// <summary>
	/// Represents interface that can create <see cref="MetaContext"/> and resolve list title for the specified member of this context.
	/// </summary>
	[PublicAPI]
	public interface IMappingSource : IMetaContextProvider, IListUrlResolver
	{
		/// <summary>
		/// Gets <see cref="Type"/> of the associated Data Context class.
		/// </summary>
		Type ContextType { get; }
	}
}