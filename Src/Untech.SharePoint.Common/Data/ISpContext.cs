using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Mappings;
using Untech.SharePoint.MetaModels;

namespace Untech.SharePoint.Data
{
	/// <summary>
	/// Represents interface for SP data context types.
	/// </summary>
	[PublicAPI]
	public interface ISpContext
	{
		/// <summary>
		/// Gets <see cref="IMappingSource"/> that is used by this instance of the <see cref="ISpContext"/>
		/// </summary>
		[NotNull]
		IMappingSource MappingSource { get; }

		/// <summary>
		/// Gets <see cref="MetaContext"/> that is used by this instance of the <see cref="ISpContext"/>
		/// </summary>
		[NotNull]
		MetaContext Model { get; }
	}
}