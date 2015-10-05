using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data
{
	[SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
	internal class OpUtils
	{
		#region [String Methods Infos]

		public static readonly MethodInfo StrContains = GetMethodInfo(() => string.Empty.Contains(default(string)));
		public static readonly MethodInfo StrStartsWith = GetMethodInfo(() => string.Empty.StartsWith(default(string)));
		public static readonly MethodInfo StrIsNullOrEmpty = GetMethodInfo(() => string.IsNullOrEmpty(default(string)));

		#endregion


		#region [Enumerable Methods Infos]

		public static readonly MethodInfo EWhere = GetMethodInfo(() => Enumerable.Where(null, default(Func<int, bool>)));
		public static readonly MethodInfo EAny = GetMethodInfo(() => Enumerable.Any<int>(null));
		public static readonly MethodInfo EAnyP = GetMethodInfo(() => Enumerable.Any<int>(null, null));
		public static readonly MethodInfo EAll = GetMethodInfo(() => Enumerable.All<int>(null, null));
		public static readonly MethodInfo EContains = GetMethodInfo(() => Enumerable.Contains(null, default(int)));

		#endregion


		#region [Queryable Methods Infos]

		public static readonly MethodInfo QAsQueryable = GetMethodInfo(() => Queryable.AsQueryable<int>(null));

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
		public static readonly MethodInfo QFirst = GetMethodInfo(() => Queryable.First<int>(null));
		public static readonly MethodInfo QLast = GetMethodInfo(() => Queryable.Last<int>(null));

		public static readonly MethodInfo QSingleOrDefault = GetMethodInfo(() => Queryable.SingleOrDefault<int>(null));
		public static readonly MethodInfo QFirstOrDefault = GetMethodInfo(() => Queryable.FirstOrDefault<int>(null));
		public static readonly MethodInfo QLastOrDefault = GetMethodInfo(() => Queryable.LastOrDefault<int>(null));

		public static readonly MethodInfo QReverse = GetMethodInfo(() => Queryable.Reverse<int>(null));

		public static readonly MethodInfo QCount = GetMethodInfo(() => Queryable.Count<int>(null));

		public static readonly MethodInfo QElementAt = GetMethodInfo(() => Queryable.ElementAt<int>(null, 0));
		public static readonly MethodInfo QElementAtOrDefault = GetMethodInfo(() => Queryable.ElementAtOrDefault<int>(null, 0));

		#endregion


		#region [SpQueryable Methods Infos]

		public static readonly MethodInfo SpqGetItems = GetMethodInfo(() => SpQueryable.GetSpListItems<int>(null, null));

		public static readonly MethodInfo SpqTakeItems = GetMethodInfo(() => SpQueryable.TakeSpListItems<int>(null, null, 0));
		public static readonly MethodInfo SpqSkipItems = GetMethodInfo(() => SpQueryable.SkipSpListItems<int>(null, null, 0));

		public static readonly MethodInfo SpqFirstItem = GetMethodInfo(() => SpQueryable.FirstSpListItem<int>(null, null, false, false));

		public static readonly MethodInfo SpqElementAtItems = GetMethodInfo(() => SpQueryable.ElementAtSpListItem<int>(null, null, 0, false));

		public static readonly MethodInfo SpqAnyItems = GetMethodInfo(() => SpQueryable.AnySpListItems(null, null));
		public static readonly MethodInfo SpqCountItems = GetMethodInfo(() => SpQueryable.CountSpListItems(null, null));

		#endregion


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
