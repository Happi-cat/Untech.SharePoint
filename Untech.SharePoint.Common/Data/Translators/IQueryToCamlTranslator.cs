using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Data.Translators
{
	public interface IQueryToCamlTranslator
	{
		string Translate(QueryModel query);
	}
}