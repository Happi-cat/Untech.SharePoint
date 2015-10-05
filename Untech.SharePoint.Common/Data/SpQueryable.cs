using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data
{
	internal static class SpQueryable
	{

		internal static IEnumerable<T> GetSpListItems<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static MethodCallExpression MakeGetSpListItems(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(OpUtils.SpqGetItems.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}

		internal static MethodCallExpression MakeAsQueryable(Type entityType, Expression source)
		{
			return Expression.Call(OpUtils.QAsQueryable.MakeGenericMethod(entityType), source);
		}

		internal static IEnumerable<T> TakeSpListItems<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static MethodCallExpression MakeTakeSpListItems(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(OpUtils.SpqTakeItems.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));
		}

		internal static IEnumerable<T> SkipSpListItems<T>(ISpItemsProvider itemsProvider, QueryModel queryModel, int count)
		{
			throw new NotImplementedException();
		}

		internal static MethodCallExpression MakeSkipSpListItems(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel, int count)
		{
			return Expression.Call(OpUtils.SpqSkipItems.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(count));
		}

		internal static T FirstSpListItem<T>(ISpItemsProvider itemsProvider, QueryModel queryModel, bool throwIfNothing, bool throwIfMultiple)
		{
			throw new NotImplementedException();
		}

		internal static MethodCallExpression MakeFirstSpListItems(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel, bool throwIfNothing, bool throwIfMultiple)
		{
			return Expression.Call(OpUtils.SpqFirstItem.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(throwIfNothing),
				Expression.Constant(throwIfMultiple));
		}

		internal static T ElementAtSpListItem<T>(ISpItemsProvider itemsProvider, QueryModel queryModel, int index, bool throwIfNothing)
		{
			throw new NotImplementedException();
		}

		internal static MethodCallExpression MakeElementAtSpListItems(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel, int index, bool throwIfNothing)
		{
			return Expression.Call(OpUtils.SpqElementAtItems.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(index),
				Expression.Constant(throwIfNothing));
		}

		internal static bool AnySpListItems(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static MethodCallExpression MakeAnySpListItems(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(OpUtils.SpqAnyItems.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}

		internal static int CountSpListItems(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			throw new NotImplementedException();
		}

		internal static MethodCallExpression MakeCountSpListItems(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(OpUtils.SpqCountItems.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}
	}
}
