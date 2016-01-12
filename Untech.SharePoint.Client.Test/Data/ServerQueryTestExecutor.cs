using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Client.Test.Data
{
	public class ClientQueryTestExecutor<T> : QueryTestExecutor<T>
	{
		public List SpList { get; set; }

		public override void MeasureCaml(string caml)
		{
			var query = new CamlQuery {ViewXml = caml};

			CamlQueryFetchTimer.Start();
			
			var result = SpList.GetItems(query);
			SpList.Context.Load(result);
			SpList.Context.ExecuteQuery();

			CamlQueryFetchTimer.Stop();
		}
	}
}