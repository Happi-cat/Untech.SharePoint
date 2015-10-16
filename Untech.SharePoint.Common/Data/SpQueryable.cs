using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Data.Translators;

namespace Untech.SharePoint.Common.Data
{
	internal static class SpQueryable
	{
		internal static IQueryable<T> FakeGetAll<T>(ISpItemsProvider itemsProvider)
		{
			throw new NotImplementedException("If you see that exception it means that expression tree rewrite failed");
		}

		internal static MethodCallExpression MakeFakeGetAll(Type entityType, ISpItemsProvider itemsProvider)
		{
			return Expression.Call(MethodUtils.SpqFakeGetAll.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)));
		}

		internal static IEnumerable<T> GetAll<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return itemsProvider.GetItems<T>(ConvertToCaml<T>(itemsProvider, queryModel));
		}

		internal static MethodCallExpression MakeGetAll(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqGetAll.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}

		internal static MethodCallExpression MakeAsQueryable(Type entityType, Expression source)
		{
			return Expression.Call(MethodUtils.QAsQueryable.MakeGenericMethod(entityType), source);
		}

		internal static IEnumerable<T> Take<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return GetAll<T>(itemsProvider, queryModel);
		}

		internal static MethodCallExpression MakeTake(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqTake.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));
		}

		internal static IEnumerable<T> Skip<T>(ISpItemsProvider itemsProvider, QueryModel queryModel, int count)
		{
			return GetAll<T>(itemsProvider, queryModel).Skip(count);
		}

		internal static MethodCallExpression MakeSkip(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel, int count)
		{
			return Expression.Call(MethodUtils.SpqSkip.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(count));
		}

		internal static T First<T>(ISpItemsProvider itemsProvider, QueryModel queryModel, bool throwIfNothing, bool throwIfMultiple)
		{
			var query = ConvertToCaml<T>(itemsProvider, queryModel);

			var item = throwIfMultiple
				? itemsProvider.SingleOrDefault<T>(query)
				: itemsProvider.FirstOrDefault<T>(query);

			if (throwIfNothing && item == null)
			{
				throw new Exception("Not found");
			}

			return item;
		}

		internal static MethodCallExpression MakeFirst(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel, bool throwIfNothing, bool throwIfMultiple)
		{
			return Expression.Call(MethodUtils.SpqFirst.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(throwIfNothing),
				Expression.Constant(throwIfMultiple));
		}

		internal static T ElementAt<T>(ISpItemsProvider itemsProvider, QueryModel queryModel, int index, bool throwIfNothing)
		{
			var query = ConvertToCaml<T>(itemsProvider, queryModel);

			var item = itemsProvider.ElementAtOrDefault<T>(query, index);

			if (throwIfNothing && item == null)
			{
				throw new Exception("Not found");
			}

			return item;
		}

		internal static MethodCallExpression MakeElementAt(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel, int index, bool throwIfNothing)
		{
			return Expression.Call(MethodUtils.SpqElementAt.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)),
				Expression.Constant(index),
				Expression.Constant(throwIfNothing));
		}

		internal static bool Any<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			queryModel.RowLimit = 1;

			return itemsProvider.Any(ConvertToCaml<T>(itemsProvider, queryModel));
		}

		internal static MethodCallExpression MakeAny(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqAny.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}

		internal static int Count<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return itemsProvider.Count(ConvertToCaml<T>(itemsProvider, queryModel));
		}

		internal static MethodCallExpression MakeCount(Type entityType, ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			return Expression.Call(MethodUtils.SpqCount.MakeGenericMethod(entityType),
				Expression.Constant(itemsProvider, typeof(ISpItemsProvider)),
				Expression.Constant(queryModel, typeof(QueryModel)));

		}

		private static string ConvertToCaml<T>(ISpItemsProvider itemsProvider, QueryModel queryModel)
		{
			var translator = new CamlQueryTranslator(itemsProvider.List)
			{
				ContentTypeId = GetContentTypeId<T>(itemsProvider)
			};
			
			return translator.Translate(queryModel);
		}

		private static string GetContentTypeId<T>(ISpItemsProvider itemsProvider)
		{
			var list = itemsProvider.List;

			return list.ContentTypes[typeof(T)].Id;
		}
	}
}
