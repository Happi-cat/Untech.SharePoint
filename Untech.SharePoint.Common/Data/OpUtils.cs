using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Common.Data
{
	internal class OpUtils
	{
		public readonly static MethodInfo E_Where = GetMethodInfo(() => Enumerable.Where(default(IEnumerable<int>), default(Func<int, bool>)));


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
	}
}
