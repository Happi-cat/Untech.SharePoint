using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Server.Test.Data
{
	public class ServerQueryTestExecutor<T> : QueryTestExecutor<T>
	{
		public SPList SpList { get; set; }

		public override void MeasureCaml(string caml)
		{
			var query = new SPQuery {ViewXml = caml};

			CamlQueryFetchTimer.Start();
			
			// ReSharper disable once UnusedVariable
			var result = SpList.GetItems(query)
				.OfType<SPListItem>()
				.ToList();

			CamlQueryFetchTimer.Stop();
		}
	}
}