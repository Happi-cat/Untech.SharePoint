using System;
using System.Xml.Linq;
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

		internal static CamlQuery CamlStringToSPQuery(string caml, uint overrideRowLimit)
		{
			throw new NotImplementedException();

			return new CamlQuery
			{
				ViewXml = caml
			};
		}
	}
}