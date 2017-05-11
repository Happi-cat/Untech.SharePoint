using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.TestTools.QueryTests
{
	public interface ITestQueryProvider<T>
	{
		IEnumerable<Func<IQueryable<T>, object>> GetQueries();
	}
}