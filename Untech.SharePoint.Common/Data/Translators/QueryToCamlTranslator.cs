using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data.Translators
{
	public class QueryToCamlTranslator : IQueryToCamlTranslator
	{
		public QueryToCamlTranslator(MetaList list)
		{
			List = list;
		}

		public MetaList List { get; set; }
		public string Translate(QueryModel query)
		{
			throw new System.NotImplementedException();
		}

	}
}