using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.TestTools.QueryTests;

namespace Untech.SharePoint.Client.Data
{
	public class ClientTestQueryExecutor<T> : PerfTestQueryExecutor<T>
	{
		public ClientTestQueryExecutor(MetaList metaList)
			: base(metaList)
		{
		}

		public List SpList { get; set; }

		public override List<object> MeasureCaml(string caml, string[] viewFields)
		{
			var query = new CamlQuery { ViewXml = string.Format(caml, ContentType) };

			CamlQueryFetchTimer.Start();

			var spItems = SpList.GetItems(query);
			SpList.Context.Load(spItems);
			SpList.Context.ExecuteQuery();

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