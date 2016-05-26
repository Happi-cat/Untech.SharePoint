using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
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
				var expected = (TResult)query.Query(AlternateList);

				var result = query.Comparer.Equals(actual, expected);
				Assert.IsTrue(result, "Query '{0}' is not equal to expected data", query.Query.Method.Name);
			}
			catch (Exception e)
			{
				if (query.Exception != null && query.Exception.IsInstanceOfType(e))
				{
					return;
				}

				e.Data["Query"] = query;
				e.Data["QueryName"] = query.Query.Method.Name;

				throw;
			}
		}
	}
}