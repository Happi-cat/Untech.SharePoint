using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class TestQuery<T, TResult> : ITestQueryAcceptor<T>
	{
		public TestQuery(Func<IQueryable<T>, object> query, IEqualityComparer<TResult> comparer = null, 
			Type exception = null, string caml = null, string[] viewFields = null)
		{
			Query = query;
			Comparer = comparer ?? EqualityComparer<TResult>.Default;
			Exception = exception;
			Caml = caml;
			ViewFields = viewFields;
		}

		public Func<IQueryable<T>, object> Query { get; private set; }

		public IEqualityComparer<TResult> Comparer { get; private set; }

		public Type Exception { get; private set; }

		public string Caml { get; private set; }

		public string[] ViewFields { get; private set; }

		public void Accept(ITestQueryExcecutor<T> executor)
		{
			executor.Visit(this);
		}
	}
}