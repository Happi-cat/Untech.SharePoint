using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Server.Utils
{
	internal static class CamlUtility
	{
		internal static SPQuery CamlStringToSPQuery(string caml)
		{
			var xCaml = XElement.Parse(caml);
			var xRowLimit = xCaml.Element("RowLimit");
			var xViewFields = xCaml.Element("ViewFields");
			var xQuery = xCaml.Element("Query") ?? new XElement("Query");

			var spQuery = new SPQuery
			{
				QueryThrottleMode = SPQueryThrottleOption.Override,
				Query = xQuery.Elements().JoinToString(string.Empty)
			};

			if (xRowLimit != null)
			{
				spQuery.RowLimit = Convert.ToUInt32(xRowLimit.Value);
			}

			if (xViewFields != null)
			{
				spQuery.ViewFieldsOnly = true;
				
				spQuery.ViewFields = xViewFields
					.Elements("FieldRef")
					.JoinToString(string.Empty);
			}

			return spQuery;
		}
	}
}