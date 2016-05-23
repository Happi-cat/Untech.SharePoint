using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public interface ITestQueryExcecutor<out T>
	{
		void Visit<TResult>(Func<IQueryable<T>, object> query, IEqualityComparer<TResult> comparer, Type exception, string caml);
	}
}