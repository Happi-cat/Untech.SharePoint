using Microsoft.SharePoint;

namespace Untech.SharePoint.Server.Utils
{
	internal static class CamlUtility
	{
		internal static SPQuery CamlStringToSPQuery(string caml)
		{
			return new SPQuery
			{
				QueryThrottleMode = SPQueryThrottleOption.Override,
				ViewXml = caml
			};
		}
	}
}