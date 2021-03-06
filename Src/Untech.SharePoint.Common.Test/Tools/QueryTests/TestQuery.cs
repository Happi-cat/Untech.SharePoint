﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class TestQuery<T, TResult> : ITestQuery<T>
	{
		public TestQuery(Func<IQueryable<T>, object> query, IEqualityComparer<TResult> comparer = null)
		{
			Query = query;
			Comparer = comparer ?? EqualityComparer<TResult>.Default;
		}

		public Func<IQueryable<T>, object> Query { get; private set; }

		public IEqualityComparer<TResult> Comparer { get; private set; }

		public Type Exception { get; set; }

		public string Caml { get; set; }

		public string[] ViewFields { get; set; }
		public bool EmptyResult { get; set; }

		public void Accept(ITestQueryExcecutor<T> executor)
		{
			executor.Visit(this);
		}
	}
}