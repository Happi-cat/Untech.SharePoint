using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Data
{
	internal static class SpQueryable
	{

		internal static IQueryable<T> GetSpListItems<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static MethodCallExpression MakeGetSpListItems(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(OpUtils.SpqGetItems.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}

		internal static IEnumerable<T> SkipSpListItems<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static IEnumerable<T> TakeSpListItems<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static T FirstSpListItem<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static T LastSpListItem<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static T ElementAtSpListItem<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static bool AnySpListItems(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static int CountSpListItems(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}
	}
}
