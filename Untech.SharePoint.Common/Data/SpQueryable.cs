using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Data
{
	internal static class SpQueryable
	{
		[UsedImplicitly]
		internal static IQueryable<T> FakeFetch<T>(ISpListItemsProvider listItemsProvider)
		{
			throw new NotImplementedException("If you see that exception it means that expression tree rewrite failed");
		}

		internal static MethodCallExpression MakeFakeFetch(Type entityType, ISpListItemsProvider listItemsProvider)
		{
			return Expression.Call(MethodUtils.SpqFakeFetch.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)));
		}

		internal static IEnumerable<T> Fetch<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return listItemsProvider.Fetch<T>(queryModel);
		}

		internal static MethodCallExpression MakeFetch(Type entityType, ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqFetch.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}

		internal static MethodCallExpression MakeAsQueryable(Type entityType, Expression source)
		{
			return Expression.Call(MethodUtils.QAsQueryable.MakeGenericMethod(entityType), source);
		}

		internal static IEnumerable<T> Take<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return Fetch<T>(listItemsProvider, queryModel);
		}

		internal static MethodCallExpression MakeTake(Type entityType, ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqTake.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));
		}

		internal static IEnumerable<T> Skip<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel, int count)
		{
			return Fetch<T>(listItemsProvider, queryModel).Skip(count);
		}

		internal static MethodCallExpression MakeSkip(Type entityType, ISpListItemsProvider listItemsProvider, QueryModel queryModel, int count)
		{
			return Expression.Call(MethodUtils.SpqSkip.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(count));
		}

		[UsedImplicitly]
		internal static T First<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel, bool throwIfNothing, bool throwIfMultiple)
		{
			var item = throwIfMultiple
				? listItemsProvider.SingleOrDefault<T>(queryModel)
				: listItemsProvider.FirstOrDefault<T>(queryModel);

			if (throwIfNothing && item == null)
			{
				throw NoMatch();
			}

			return item;
		}

		internal static MethodCallExpression MakeFirst(Type entityType, ISpListItemsProvider listItemsProvider, QueryModel queryModel, bool throwIfNothing, bool throwIfMultiple)
		{
			return Expression.Call(MethodUtils.SpqFirst.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(throwIfNothing),
				Expression.Constant(throwIfMultiple));
		}

		[UsedImplicitly]
		internal static T ElementAt<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel, int index, bool throwIfNothing)
		{
			var item = listItemsProvider.ElementAtOrDefault<T>(queryModel, index);

			if (throwIfNothing && item == null)
			{
				throw NoMatch();
			}

			return item;
		}

		internal static MethodCallExpression MakeElementAt(Type entityType, ISpListItemsProvider listItemsProvider, QueryModel queryModel, int index, bool throwIfNothing)
		{
			return Expression.Call(MethodUtils.SpqElementAt.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(index),
				Expression.Constant(throwIfNothing));
		}

		internal static bool Any<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			queryModel.RowLimit = 1;

			return listItemsProvider.Any<T>(queryModel);
		}

		internal static MethodCallExpression MakeAny(Type entityType, ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqAny.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}

		internal static int Count<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return listItemsProvider.Count<T>(queryModel);
		}

		internal static MethodCallExpression MakeCount(Type entityType, ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqCount.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));
		}

		internal static IEnumerable<TResult> Select<TContentType, TResult>(ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, Func<TContentType, TResult> selector)
		{
			return listItemsProvider.Fetch<TContentType>(queryModel)
				.Select(selector);
		}

		internal static MethodCallExpression MakeSelect(Type contentType, Type resulType,  ISpListItemsProvider listItemsProvider, QueryModel queryModel, LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqSelect.MakeGenericMethod(contentType, resulType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(selector.Compile()));
		}

		internal static TResult Min<TContentType, TResult>(ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, Func<TContentType, TResult> selector)
		{
			return listItemsProvider.Fetch<TContentType>(queryModel)
				.Select(selector)
				.Min();
		}

		internal static MethodCallExpression MakeMin(Type contentType, Type resulType, ISpListItemsProvider listItemsProvider, QueryModel queryModel, LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqMin.MakeGenericMethod(contentType, resulType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(selector.Compile()));
		}

		internal static TResult Max<TContentType, TResult>(ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, Func<TContentType, TResult> selector)
		{
			return listItemsProvider.Fetch<TContentType>(queryModel)
				.Select(selector)
				.Max();
		}

		internal static MethodCallExpression MakeMax(Type contentType, Type resulType, ISpListItemsProvider listItemsProvider, QueryModel queryModel, LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqMax.MakeGenericMethod(contentType, resulType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(selector.Compile()));
		}

		private static Exception NoMatch()
		{
			return new InvalidOperationException("No match");
		}
	}
}
