using System;
using System.Collections.Generic;
using System.Linq;
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
				ViewXml = string.Format("<View>{0}</View>",caml)
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

		internal static IReadOnlyCollection<string> GetViewFields(string caml)
		{
			var xCaml = XElement.Parse(caml);
			var xViewFields = xCaml.Element("ViewFields");
			if (xViewFields != null)
			{
				return xViewFields.Descendants("FieldRef")
					.Attributes("Name")
					.Select(n => n.Value)
					.ToList();
			}
			return null;
		}
	}
}