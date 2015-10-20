using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Server.Utils
{
	internal static class CamlUtility
	{
		private static readonly XName[] AllowedQueryTags = { "Where", "OrderBy", "GroupBy" };

		internal static SPQuery CamlStringToSPQuery(string caml)
		{
			var xCaml = XElement.Parse(caml);
			var xRowLimit = xCaml.Element("RowLimit");
			var xViewFields = xCaml.Element("ViewFields");

			var spQuery = new SPQuery
			{
				QueryThrottleMode = SPQueryThrottleOption.Override,
				Query = xCaml.Elements()
					.Where(n => n.Name.In(AllowedQueryTags))
					.JoinToString(string.Empty)
			};

			if (xRowLimit != null)
			{
				spQuery.RowLimit = Convert.ToUInt32(xRowLimit.Value);
			}

			if (xViewFields != null)
			{
				spQuery.ViewFields = xViewFields.Elements("FieldRef")
					.JoinToString(string.Empty);
			}

			return spQuery;
		}
	}
}