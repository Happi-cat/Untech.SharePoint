using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data
{
	public interface IDataContext
	{
		ClientContext ClientContext { get; }

		MetaModel Mapping { get; }
	}
}