using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Data
{
	internal static class SpQueryable
	{

		internal static IEnumerable<T> GetSpListItems<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static TResult SkipSpListItems<TResult>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static bool AnySpListItems(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}
	}
}
