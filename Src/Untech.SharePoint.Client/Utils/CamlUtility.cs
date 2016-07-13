using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Utils
{
	internal static class CamlUtility
	{
		internal static CamlQuery CamlStringToSPQuery(string caml)
		{
			return new CamlQuery
			{
				ViewXml = caml
			};
		}
	}
}