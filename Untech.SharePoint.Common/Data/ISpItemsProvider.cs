using System.Collections.Generic;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Data
{
	public interface ISpItemsProvider
	{
		IEnumerable<T> GetItems<T>(QueryModel queryModel);

		bool AnyItem(QueryModel queryModel);
	}
}