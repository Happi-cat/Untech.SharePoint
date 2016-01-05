using System;
using System.Linq;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class QueryTestExceptionWrapper<T, TException> : QueryTest<T>
		where TException: Exception
	{
		public QueryTestExceptionWrapper(QueryTest<T> inner)
		{
			Inner = inner;
		}

		public QueryTest<T> Inner { get; private set; }

		public override void Test(ISpList<T> list, IQueryable<T> alternateList)
		{
			CustomAssert.Throw<TException>(() => Inner.Test(list, alternateList));
		}

		public override TimeSpan GetElapsedTime()
		{
			return Inner.GetElapsedTime();
		}

		public override int GetItemsCounter()
		{
			return Inner.GetItemsCounter();
		}

		public override string ToString()
		{
			return Inner.ToString();
		}
	}
}