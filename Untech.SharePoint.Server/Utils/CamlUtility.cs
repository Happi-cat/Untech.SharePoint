using System;
using System.Collections.Generic;
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
				spQuery.ViewFieldsOnly = true;
				
				spQuery.ViewFields = xViewFields
					.Elements("FieldRef")
					.JoinToString(string.Empty);
			}

			return spQuery;
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
			return new List<string>();
		}
	}
}