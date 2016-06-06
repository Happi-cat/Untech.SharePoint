using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	internal static class SpQueryable
	{
		#region [Fetch]

		[UsedImplicitly]
		internal static IQueryable<T> FakeFetch<T>(ISpListItemsProvider listItemsProvider)
		{
			throw new NotImplementedException("If you see that exception it means that expression tree rewrite failed");
		}

		internal static MethodCallExpression MakeFakeFetch(Type entityType, ISpListItemsProvider listItemsProvider)
		{
			return Expression.Call(MethodUtils.SpqFakeFetch.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)));
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

		#endregion

		#region [Take]

		internal static IEnumerable<T> Take<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return Fetch<T>(listItemsProvider, queryModel);
		}

		internal static IEnumerable<TResult> Take<TSoure, TResult>(ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, Func<TSoure, TResult> selector)
		{
			return Fetch<TSoure>(listItemsProvider, queryModel).Select(selector);
		}

		internal static MethodCallExpression MakeTake(Type entityType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqTake.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)));
		}

		internal static MethodCallExpression MakeTake(Type entityType, Type resulType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqTakeP.MakeGenericMethod(entityType, resulType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)),
				Expression.Constant(selector.Compile()));
		}

		#endregion

		#region [First]

		internal static T First<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel, bool throwIfNothing,
			bool throwIfMultiple)
		{
			var item = throwIfMultiple
				? listItemsProvider.SingleOrDefault<T>(queryModel)
				: listItemsProvider.FirstOrDefault<T>(queryModel);

			if (throwIfNothing && item == null)
			{
				throw Error.NoMatch();
			}

			return item;
		}

		internal static TResult First<TSource, TResult>(ISpListItemsProvider listItemsProvider, QueryModel queryModel,
			bool throwIfNothing, bool throwIfMultiple, Func<TSource, TResult> selector)
		{
			var item = First<TSource>(listItemsProvider, queryModel, throwIfNothing, throwIfMultiple);
			return item == null ? default(TResult) : selector(item);
		}

		internal static MethodCallExpression MakeFirst(Type entityType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, bool throwIfNothing, bool throwIfMultiple)
		{
			return Expression.Call(MethodUtils.SpqFirst.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)),
				Expression.Constant(throwIfNothing),
				Expression.Constant(throwIfMultiple));
		}

		internal static MethodCallExpression MakeFirst(Type entityType, Type resultType,
			ISpListItemsProvider listItemsProvider, QueryModel queryModel, bool throwIfNothing, bool throwIfMultiple,
			LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqFirstP.MakeGenericMethod(entityType, resultType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)),
				Expression.Constant(throwIfNothing),
				Expression.Constant(throwIfMultiple),
				Expression.Constant(selector.Compile()));
		}

		#endregion

		#region [Element At]

		internal static T ElementAt<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel, int index,
			bool throwIfNothing)
		{
			var item = listItemsProvider.ElementAtOrDefault<T>(queryModel, index);

			if (throwIfNothing && item == null)
			{
				throw Error.NoMatch();
			}

			return item;
		}

		internal static TResult ElementAt<TSource, TResult>(ISpListItemsProvider listItemsProvider, QueryModel queryModel,
			int index, bool throwIfNothing, Func<TSource, TResult> selector)
		{
			var item = ElementAt<TSource>(listItemsProvider, queryModel, index, throwIfNothing);

			if (item == null)
			{
				return default(TResult);
			}

			return selector(item);
		}


		internal static MethodCallExpression MakeElementAt(Type entityType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, int index, bool throwIfNothing)
		{
			return Expression.Call(MethodUtils.SpqElementAt.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)),
				Expression.Constant(index),
				Expression.Constant(throwIfNothing));
		}

		internal static MethodCallExpression MakeElementAt(Type entityType, Type projectionType,
			ISpListItemsProvider listItemsProvider, QueryModel queryModel, int index, bool throwIfNothing,
			LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqElementAtP.MakeGenericMethod(entityType, projectionType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)),
				Expression.Constant(index),
				Expression.Constant(throwIfNothing),
				Expression.Constant(selector.Compile()));
		}

		#endregion

		#region [Select]

		internal static IEnumerable<TResult> Select<TSource, TResult>(ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, Func<TSource, TResult> selector)
		{
			return listItemsProvider.Fetch<TSource>(queryModel)
				.Select(selector);
		}

		internal static MethodCallExpression MakeSelect(Type contentType, Type resulType,
			ISpListItemsProvider listItemsProvider, QueryModel queryModel, LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqSelect.MakeGenericMethod(contentType, resulType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)),
				Expression.Constant(selector.Compile()));
		}

		#endregion

		#region [Aggregates]

		internal static bool Any<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			queryModel.RowLimit = 1;

			return listItemsProvider.Any<T>(queryModel);
		}

		internal static MethodCallExpression MakeAny(Type entityType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqAny.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)));

		}

		internal static bool NotAny<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			queryModel.RowLimit = 1;

			return !listItemsProvider.Any<T>(queryModel);
		}

		internal static MethodCallExpression MakeNotAny(Type entityType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqAll.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}


		internal static int Count<T>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return listItemsProvider.Count<T>(queryModel);
		}

		internal static MethodCallExpression MakeCount(Type entityType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqCount.MakeGenericMethod(entityType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)));
		}

		internal static TResult Min<TSource, TResult>(ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, Func<TSource, TResult> selector)
		{
			return listItemsProvider.Fetch<TSource>(queryModel)
				.Min(selector);
		}

		internal static MethodCallExpression MakeMin(Type contentType, Type resulType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqMinP.MakeGenericMethod(contentType, resulType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)),
				Expression.Constant(selector.Compile()));
		}

		internal static TSource Min<TSource>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return listItemsProvider.Fetch<TSource>(queryModel)
				.Min();
		}

		internal static MethodCallExpression MakeMin(Type contentType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqMin.MakeGenericMethod(contentType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));
		}

		internal static TResult Max<TSource, TResult>(ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, Func<TSource, TResult> selector)
		{
			return listItemsProvider.Fetch<TSource>(queryModel)
				.Max(selector);
		}

		internal static MethodCallExpression MakeMax(Type contentType, Type resulType, ISpListItemsProvider listItemsProvider,
			QueryModel queryModel, LambdaExpression selector)
		{
			return Expression.Call(MethodUtils.SpqMaxP.MakeGenericMethod(contentType, resulType),
				Expression.Constant(listItemsProvider, typeof (ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof (QueryModel)),
				Expression.Constant(selector.Compile()));
		}

		internal static TSource Max<TSource>(ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return listItemsProvider.Fetch<TSource>(queryModel)
				.Max();
		}

		internal static MethodCallExpression MakeMax(Type contentType, ISpListItemsProvider listItemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqMax.MakeGenericMethod(contentType),
				Expression.Constant(listItemsProvider, typeof(ISpListItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));
		}

		#endregion

	}
}
