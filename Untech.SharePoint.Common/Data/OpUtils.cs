using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data
{
	[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
	internal class OpUtils
	{
		public static readonly MethodInfo StrContains = GetMethodInfo(() => string.Empty.Contains(default(string)));
		public static readonly MethodInfo StrStartsWith = GetMethodInfo(() => string.Empty.StartsWith(default(string)));
		public static readonly MethodInfo StrIsNullOrEmpty = GetMethodInfo(() => string.IsNullOrEmpty(default(string)));

		public static readonly MethodInfo EWhere = GetMethodInfo(() => default(IEnumerable<int>).Where(default(Func<int, bool>)));
		public static readonly MethodInfo EAny = GetMethodInfo(() => Enumerable.Any(default(IEnumerable<int>)));
		public static readonly MethodInfo EAnyP = GetMethodInfo(() => Enumerable.Any(default(IEnumerable<int>), default(Func<int, bool>)));
		public static readonly MethodInfo EAll = GetMethodInfo(() => Enumerable.All(default(IEnumerable<int>), default(Func<int, bool>)));
		public static readonly MethodInfo EContains = GetMethodInfo(() => Enumerable.Contains(default(IEnumerable<int>), default(int)));

		public static readonly MethodInfo QAsQueryable = GetMethodInfo(() => Queryable.AsQueryable(default(IEnumerable<int>)));
		public static readonly MethodInfo QWhere = GetMethodInfo(() => Queryable.Where(default(IQueryable<int>), default(Expression<Func<int, bool>>)));
		public static readonly MethodInfo QAny = GetMethodInfo(() => Queryable.Any(default(IQueryable<int>)));
		public static readonly MethodInfo QAnyP = GetMethodInfo(() => Queryable.Any(default(IQueryable<int>), default(Expression<Func<int, bool>>)));
		public static readonly MethodInfo QAll = GetMethodInfo(() => Queryable.All(default(IQueryable<int>), default(Expression<Func<int, bool>>)));

		public static readonly MethodInfo QOrderBy =
			GetMethodInfo(() => Queryable.OrderBy(default(IQueryable<int>), default(Expression<Func<int, int>>)));
		public static readonly MethodInfo QOrderByDescending =
			GetMethodInfo(() => Queryable.OrderByDescending(default(IQueryable<int>), default(Expression<Func<int, int>>)));

		public static readonly MethodInfo QThenBy = GetMethodInfo(() => Queryable.ThenBy<int, int>(null, null));
		public static readonly MethodInfo QThenrByDescending = GetMethodInfo(() => Queryable.ThenByDescending(default(IOrderedQueryable<int>), default(Expression<Func<int, int>>)));

		public static readonly MethodInfo QTake = GetMethodInfo(() => Queryable.Take(default(IQueryable<int>), 0));
		
		public static readonly MethodInfo QSignle = GetMethodInfo(() => Queryable.Single(default(IQueryable<int>)));
		public static readonly MethodInfo QFirst= GetMethodInfo(() => Queryable.First(default(IQueryable<int>)));
		public static readonly MethodInfo QLast = GetMethodInfo(() => Queryable.Last(default(IQueryable<int>))); 
		
		public static readonly MethodInfo QSingleOrDefault = GetMethodInfo(() => Queryable.SingleOrDefault(default(IQueryable<int>))); 
		public static readonly MethodInfo QFirstOrDefault = GetMethodInfo(() => Queryable.FirstOrDefault(default(IQueryable<int>)));
		public static readonly MethodInfo QLastOrDefault= GetMethodInfo(() => Queryable.LastOrDefault(default(IQueryable<int>)));
		
		public static readonly MethodInfo QReverse= GetMethodInfo(() => Queryable.Reverse(default(IQueryable<int>)));

		public static readonly MethodInfo SpqGetItems = GetMethodInfo(() => SpQueryable.GetSpListItems<int>(default(ISpItemsProvider), default(QueryModel)));

		public static readonly MethodInfo SpqTakeItems = GetMethodInfo(() => SpQueryable.TakeSpListItems<int>(null, null));
		public static readonly MethodInfo SpqSkipItems = GetMethodInfo(() => SpQueryable.SkipSpListItems<int>(null, null));

		public static readonly MethodInfo SpqFirstItem = GetMethodInfo(() => SpQueryable.FirstSpListItem<int>(null, null));
		public static readonly MethodInfo SpqLastItem = GetMethodInfo(() => SpQueryable.LastSpListItem<int>(null, null));

		public static readonly MethodInfo SpqAnyItems = GetMethodInfo(() => SpQueryable.AnySpListItems(null, null));
		public static readonly MethodInfo SpqCountItems = GetMethodInfo(() => SpQueryable.CountSpListItems(null, null));

		public static readonly MethodInfo ObjIn = GetMethodInfo(() => default(object).In(null));

		public static MethodInfo GetMethodInfo<TResult>(Expression<Func<TResult>> expression)
		{
			var methodCall = (MethodCallExpression)expression.Body;
			var method = methodCall.Method;
			return method.IsGenericMethod ? method.GetGenericMethodDefinition() : method;
		}

		public static PropertyInfo GetPropertyInfo<TResult>(Expression<Func<TResult>> expression)
		{
			var memberAccess = (MemberExpression)expression.Body;
			return (PropertyInfo)memberAccess.Member;
		}

		public static bool IsOperator(MethodInfo x, MethodInfo op)
		{
			if (x.IsGenericMethod)
			{
				x = x.GetGenericMethodDefinition();
			}
			return x == op;
		}

	}
}
