using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data
{
	[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
	internal static class MethodUtils
	{
		public static readonly MethodInfo ListContains = GetMethodInfo(() => new List<int>().Contains(0));

		#region [String Methods Infos]

		public static readonly MethodInfo StrContains = GetMethodInfo(() => string.Empty.Contains(default(string)));
		public static readonly MethodInfo StrStartsWith = GetMethodInfo(() => string.Empty.StartsWith(default(string)));
		public static readonly MethodInfo StrIsNullOrEmpty = GetMethodInfo(() => string.IsNullOrEmpty(default(string)));

		#endregion


		#region [Enumerable Methods Infos]

		public static readonly MethodInfo EContains = GetMethodInfo(() => Enumerable.Contains(null, default(int)));

		#endregion


		#region [Queryable Methods Infos]

		public static readonly MethodInfo QAsQueryable = GetMethodInfo(() => Queryable.AsQueryable<int>(null));

		public static readonly MethodInfo QSelect = GetMethodInfo(() => Queryable.Select(null, default(Expression<Func<int, int>>)));

		public static readonly MethodInfo QMin = GetMethodInfo(() => Queryable.Min<int>(null));
		public static readonly MethodInfo QMinP = GetMethodInfo(() => Queryable.Min(null, default(Expression<Func<int, int>>)));
		public static readonly MethodInfo QMax = GetMethodInfo(() => Queryable.Max<int>(null));
		public static readonly MethodInfo QMaxP = GetMethodInfo(() => Queryable.Max(null, default(Expression<Func<int, int>>)));

		public static readonly MethodInfo QWhere = GetMethodInfo(() => Queryable.Where(null, default(Expression<Func<int, bool>>)));

		public static readonly MethodInfo QAny = GetMethodInfo(() => Queryable.Any<int>(null));
		public static readonly MethodInfo QAnyP = GetMethodInfo(() => Queryable.Any<int>(null, null));
		public static readonly MethodInfo QAll = GetMethodInfo(() => Queryable.All<int>(null, null));

		public static readonly MethodInfo QOrderBy = GetMethodInfo(() => Queryable.OrderBy<int, int>(null, null));
		public static readonly MethodInfo QOrderByDescending = GetMethodInfo(() => Queryable.OrderByDescending<int, int>(null, null));

		public static readonly MethodInfo QThenBy = GetMethodInfo(() => Queryable.ThenBy<int, int>(null, null));
		public static readonly MethodInfo QThenrByDescending = GetMethodInfo(() => Queryable.ThenByDescending<int, int>(null, null));

		public static readonly MethodInfo QTake = GetMethodInfo(() => Queryable.Take<int>(null, 0));
		public static readonly MethodInfo QSkip = GetMethodInfo(() => Queryable.Skip<int>(null, 0));

		public static readonly MethodInfo QSingle = GetMethodInfo(() => Queryable.Single<int>(null));
		public static readonly MethodInfo QSingleP = GetMethodInfo(() => Queryable.Single<int>(null, null));
		public static readonly MethodInfo QFirst = GetMethodInfo(() => Queryable.First<int>(null));
		public static readonly MethodInfo QFirstP = GetMethodInfo(() => Queryable.First<int>(null, null));
		public static readonly MethodInfo QLast = GetMethodInfo(() => Queryable.Last<int>(null));
		public static readonly MethodInfo QLastP = GetMethodInfo(() => Queryable.Last<int>(null, null));

		public static readonly MethodInfo QSingleOrDefault = GetMethodInfo(() => Queryable.SingleOrDefault<int>(null));
		public static readonly MethodInfo QSingleOrDefaultP = GetMethodInfo(() => Queryable.SingleOrDefault<int>(null, null));
		public static readonly MethodInfo QFirstOrDefault = GetMethodInfo(() => Queryable.FirstOrDefault<int>(null));
		public static readonly MethodInfo QFirstOrDefaultP = GetMethodInfo(() => Queryable.FirstOrDefault<int>(null, null));
		public static readonly MethodInfo QLastOrDefault = GetMethodInfo(() => Queryable.LastOrDefault<int>(null));
		public static readonly MethodInfo QLastOrDefaultP = GetMethodInfo(() => Queryable.LastOrDefault<int>(null, null));

		public static readonly MethodInfo QReverse = GetMethodInfo(() => Queryable.Reverse<int>(null));

		public static readonly MethodInfo QCount = GetMethodInfo(() => Queryable.Count<int>(null));
		public static readonly MethodInfo QCountP = GetMethodInfo(() => Queryable.Count<int>(null, null));

		public static readonly MethodInfo QElementAt = GetMethodInfo(() => Queryable.ElementAt<int>(null, 0));
		public static readonly MethodInfo QElementAtOrDefault = GetMethodInfo(() => Queryable.ElementAtOrDefault<int>(null, 0));

		#endregion


		#region [SpQueryable Methods Infos]

		public static readonly MethodInfo SpqFakeFetch = GetMethodInfo(() => SpQueryable.FakeFetch<int>(null));

		public static readonly MethodInfo SpqFetch = GetMethodInfo(() => SpQueryable.Fetch<int>(null, null));

		public static readonly MethodInfo SpqSelect = GetMethodInfo(() => SpQueryable.Select<int, int>(null, null, null));

		public static readonly MethodInfo SpqMin = GetMethodInfo(() => SpQueryable.Min<int>(null, null));
		public static readonly MethodInfo SpqMinP = GetMethodInfo(() => SpQueryable.Min<int, int>(null, null, null));
		public static readonly MethodInfo SpqMax = GetMethodInfo(() => SpQueryable.Max<int>(null, null));
		public static readonly MethodInfo SpqMaxP = GetMethodInfo(() => SpQueryable.Max<int, int>(null, null, null));

		public static readonly MethodInfo SpqTake = GetMethodInfo(() => SpQueryable.Take<int>(null, null));
		public static readonly MethodInfo SpqTakeP = GetMethodInfo(() => SpQueryable.Take<int, int>(null, null, null));

		public static readonly MethodInfo SpqFirst = GetMethodInfo(() => SpQueryable.First<int>(null, null, false, false));
		public static readonly MethodInfo SpqFirstP = GetMethodInfo(() => SpQueryable.First<int, int>(null, null, false, false, null));

		public static readonly MethodInfo SpqElementAt = GetMethodInfo(() => SpQueryable.ElementAt<int>(null, null, 0, false));
		public static readonly MethodInfo SpqElementAtP = GetMethodInfo(() => SpQueryable.ElementAt<int, int>(null, null, 0, false, null));

		public static readonly MethodInfo SpqAny = GetMethodInfo(() => SpQueryable.Any<int>(null, null));
		public static readonly MethodInfo SpqAll = GetMethodInfo(() => SpQueryable.NotAny<int>(null, null));
		public static readonly MethodInfo SpqCount = GetMethodInfo(() => SpQueryable.Count<int>(null, null));

		#endregion


		public static readonly MethodInfo ObjIn = GetMethodInfo(() => default(object).In(null));
		
		public static bool IsOperator(MethodInfo x, MethodInfo y)
		{
			return GenericMethodDefinitionComparer.Default.Equals(x, y);
		}

		private static MethodInfo GetMethodInfo<TResult>(Expression<Func<TResult>> expression)
		{
			var methodCall = (MethodCallExpression)expression.Body;
			var method = methodCall.Method;
			return method.IsGenericMethod ? method.GetGenericMethodDefinition() : method;
		}

	}
}
