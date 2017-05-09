using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.TestTools.QueryTests;

namespace Untech.SharePoint.Server.Data
{
	public class ServerTestQueryExecutor<T> : PerfTestQueryExecutor<T>
	{
		public ServerTestQueryExecutor(MetaList metaList)
			: base(metaList)
		{
		}

		public SPList SpList { get; set; }

		public override List<object> MeasureCaml(string caml, string[] viewFields)
		{
			var query = new SPQuery
			{
				ViewXml = string.Format(caml, ContentType),
				QueryThrottleMode = SPQueryThrottleOption.Override
			};
			CamlQueryFetchTimer.Start();

			// ReSharper disable once UnusedVariable
			var spItems = SpList.GetItems(query)
				.Cast<SPListItem>();

			var items = new List<object>();
			foreach (var spItem in spItems)
			{
				var itemProps = viewFields.Select(viewField => spItem[viewField]).ToList();
				items.Add(itemProps);
			}

			CamlQueryFetchTimer.Stop();

			return items;
		}
	}
}