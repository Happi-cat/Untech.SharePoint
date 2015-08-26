using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data
{
	public abstract class BaseDataContext
	{
		public ClientContext ClientContext { get; protected set; }

		public MetaModel Mapping { get; protected set; }
	}
}