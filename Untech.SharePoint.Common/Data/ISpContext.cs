using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents interface for SP data context types.
	/// </summary>
	public interface ISpContext
	{
		/// <summary>
		/// Gets <see cref="Config"/> that is used by this instance of the <see cref="ISpContext"/>
		/// </summary>
		Config Config { get; }

		/// <summary>
		/// Gets <see cref="IMappingSource"/> that is used by this instance of the <see cref="ISpContext"/>
		/// </summary>
		IMappingSource MappingSource { get; }

		/// <summary>
		/// Gets <see cref="MetaContext"/> that is used by this instance of the <see cref="ISpContext"/>
		/// </summary>
		MetaContext Model { get; }
	}
}