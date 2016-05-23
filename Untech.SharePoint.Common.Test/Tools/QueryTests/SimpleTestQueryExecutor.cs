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

		public void Visit<TResult>(Func<IQueryable<T>, object> query, IEqualityComparer<TResult> comparer, Type exception, string caml)
		{
			try
			{
				var actual = (TResult)query(List);
				var expected = (TResult)query(AlternateList);

				var result = comparer.Equals(actual, expected);
				Assert.IsTrue(result, "Query '{0}' is not equal to expected data", query.Method.Name);
			}
			catch (Exception e)
			{
				if (exception != null && exception.IsInstanceOfType(e))
				{
					return;
				}

				e.Data["Query"] = query;
				e.Data["QueryName"] = query.Method.Name;

				throw;
			}
		}
	}
}