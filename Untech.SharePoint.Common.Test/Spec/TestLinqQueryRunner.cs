using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Spec
{
	public interface ITestQueryProvider<T>
	{
		IEnumerable<TestQuery<T>> GetTestQueries();
	}

	public abstract class TestQuery<T>
	{
		protected TestQuery()
		{
			Stopwatch = new Stopwatch();
		}

		public Stopwatch Stopwatch { get; protected set; }

		public static TestQuery<T> Create<TResult>(Func<IQueryable<T>, TResult> query)
		{
			return new TestQuery<T, TResult>(query, EqualityComparer<TResult>.Default);
		}

		public static TestQuery<T> Create<TResult>(Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
		{
			return new TestQuery<T, TResult>(query, comparer);
		}

		public abstract void Test(ISpList<T> list, IQueryable<T> alternateList);

		public TestQuery<T> Throws<TException>()
			where TException : Exception
		{
			return new TestQueryExceptionWrapper<T, TException>(this);
		}
	}

	public class TestQueryExceptionWrapper<T, TException> : TestQuery<T>
		where TException: Exception
	{
		private readonly TestQuery<T> _inner;

		public TestQueryExceptionWrapper(TestQuery<T> inner)
		{
			_inner = inner;
			Stopwatch = inner.Stopwatch;
		}

		public override void Test(ISpList<T> list, IQueryable<T> alternateList)
		{
			CustomAssert.Throw<TException>(() => _inner.Test(list, alternateList));
		}

		public override string ToString()
		{
			return _inner.ToString();
		}
	}

	public class TestQuery<T, TResult> : TestQuery<T>
	{
		private readonly Func<IQueryable<T>, TResult> _query;
		private readonly IEqualityComparer<TResult> _comparer;

		public TestQuery(Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
		{
			_query = query;
			_comparer = comparer;
		}

		public override void Test(ISpList<T> list, IQueryable<T> alternateList)
		{
			try
			{
				TestInternal(list, alternateList);
			}
			catch (Exception e)
			{
				e.Data["Query"] = _query;
				e.Data["QueryName"] = _query.Method.Name;

				throw;
			}
		}

		private void TestInternal(ISpList<T> list, IQueryable<T> alternateList)
		{
			Stopwatch.Start();
			var loadedResult = _query(list);
			Stopwatch.Stop();

			var expectedResult = _query(alternateList);

			Assert.IsTrue(_comparer.Equals(loadedResult, expectedResult), "Query '{0}' is not equal to expected data",
				_query.Method.Name);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return _query.Method.Name;
		}
	}
}