using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Meta;

namespace Untech.SharePoint.Client.Data
{
	public interface IDataContext
	{
		ClientContext ClientContext { get; }

		MetaModel Mapping { get; }
	}
}