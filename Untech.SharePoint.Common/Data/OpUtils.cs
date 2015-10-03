using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data
{
	internal class OpUtils
	{
		public static readonly MethodInfo StrContains = GetMethodInfo(() => string.Empty.Contains(default(string)));
		public static readonly MethodInfo StrStartsWith = GetMethodInfo(() => string.Empty.StartsWith(default(string)));
		public static readonly MethodInfo StrIsNullOrEmpty = GetMethodInfo(() => string.IsNullOrEmpty(default(string)));

		public static readonly MethodInfo EWhere = GetMethodInfo(() => default(IEnumerable<int>).Where(default(Func<int, bool>)));
		public static readonly MethodInfo EAny = GetMethodInfo(() => default(IEnumerable<int>).Any());
		public static readonly MethodInfo EAnyP = GetMethodInfo(() => default(IEnumerable<int>).Any(default(Func<int, bool>)));
		public static readonly MethodInfo EAll = GetMethodInfo(() => default(IEnumerable<int>).All(default(Func<int, bool>)));
		public static readonly MethodInfo EContains = GetMethodInfo(() => default(IEnumerable<int>).Contains(default(int)));
		
		public static readonly MethodInfo QWhere = GetMethodInfo(() => default(IQueryable<int>).Where(default(Expression<Func<int, bool>>)));
		public static readonly MethodInfo QAny = GetMethodInfo(() => default(IQueryable<int>).Any());
		public static readonly MethodInfo QAnyP = GetMethodInfo(() => default(IQueryable<int>).Any(default(Expression<Func<int, bool>>)));
		public static readonly MethodInfo QAll = GetMethodInfo(() => default(IQueryable<int>).All(default(Expression<Func<int, bool>>)));

		public static readonly MethodInfo SpqGetItems = GetMethodInfo(() => SpQueryable.GetSpListItems<int>(default(ISpItemsProvider), default(QueryModel)));

		public static readonly MethodInfo SpqTakeItems =
			GetMethodInfo(() => SpQueryable.TakeSpListItems<int>(default(ISpItemsProvider), default(QueryModel)));
		public static readonly MethodInfo SpqSkipItems =
			GetMethodInfo(() => SpQueryable.SkipSpListItems<int>(default(ISpItemsProvider), default(QueryModel)));

		public static readonly MethodInfo SpqFirstItem =
			GetMethodInfo(() => SpQueryable.FirstSpListItem<int>(default(ISpItemsProvider), default(QueryModel)));
		public static readonly MethodInfo SpqLastItem =
			GetMethodInfo(() => SpQueryable.LastSpListItem<int>(default(ISpItemsProvider), default(QueryModel)));

		public static readonly MethodInfo SpqAnyItems = GetMethodInfo(() => SpQueryable.AnySpListItems(default(ISpItemsProvider), default(QueryModel)));
		public static readonly MethodInfo SpqCountItems = GetMethodInfo(() => SpQueryable.CountSpListItems(default(ISpItemsProvider), default(QueryModel)));

		public static readonly MethodInfo ObjIn = GetMethodInfo(() => default(object).In(default(IEnumerable<object>)));

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
