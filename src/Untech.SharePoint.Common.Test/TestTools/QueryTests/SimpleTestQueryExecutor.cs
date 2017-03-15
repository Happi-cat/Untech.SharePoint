using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.TestTools.QueryTests
{
	public class SimpleTestQueryExecutor<T> : ITestQueryExcecutor<T>
	{
		public ISpList<T> List { get; set; }

		public IQueryable<T> AlternateList { get; set; }

		public void Visit<TResult>(TestQuery<T, TResult> query)
		{
			try
			{
				var actual = (TResult)query.Query(List);
				var actualEnumerable = actual as IEnumerable;
				var expected = (TResult)query.Query(AlternateList);

				var isResultEmpty = actual == null || (actualEnumerable != null && !actualEnumerable.GetEnumerator().MoveNext());
				var comparisonResult = query.Comparer.Equals(actual, expected);
				Assert.IsTrue(comparisonResult, "Query '{0}' is not equal to expected data", query.Query.Method.Name);

				if (query.EmptyResult)
				{
					Assert.IsTrue(isResultEmpty, "Query '{0}' is not empty when should be empty.", query.Query.Method.Name);
				}
				else
				{
					Assert.IsFalse(isResultEmpty, "Query '{0}' is empty when shouldn't be empty.", query.Query.Method.Name);
				}
			}
			catch (Exception e)
			{
				if (query.Exception != null && query.Exception.IsInstanceOfType(e))
				{
					return;
				}

				e.Data["Query"] = query.Query;
				e.Data["QueryName"] = query.Query.Method.Name;

				throw;
			}
		}
	}
}