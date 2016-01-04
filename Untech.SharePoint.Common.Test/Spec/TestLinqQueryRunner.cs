using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Spec
{
	public enum TestQueryType
	{
		Supported,
		Unsupported
	}

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

		public Stopwatch Stopwatch { get; private set; }

		public static TestQuery<T> Create<TResult>(Func<IQueryable<T>, TResult> query)
		{
			return new TestQuery<T, TResult>(query, EqualityComparer<TResult>.Default);
		}

		public static TestQuery<T> Create<TResult>(Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
		{
			return new TestQuery<T, TResult>(query, comparer);
		}

		public static TestQuery<T> CreateNotSupported<TResult>(Func<IQueryable<T>, TResult> query)
		{
			return new TestQuery<T, TResult>(query, TestQueryType.Unsupported);
		}

		public abstract void Test(ISpList<T> list, IQueryable<T> alternateList);
	}

	public class TestQuery<T, TResult> : TestQuery<T>
	{
		private readonly bool _notSupported;
		private readonly Func<IQueryable<T>, TResult> _query;
		private readonly IEqualityComparer<TResult> _comparer;

		public TestQuery(Func<IQueryable<T>, TResult> query, TestQueryType type = TestQueryType.Supported)
		{
			_query = query;
			_comparer = EqualityComparer<TResult>.Default;
			_notSupported = type == TestQueryType.Unsupported;
		}

		public TestQuery(Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
		{
			_query = query;
			_comparer = comparer;
		}


		public override void Test(ISpList<T> list, IQueryable<T> alternateList)
		{
			if (_notSupported)
			{
				CustomAssert.Throw<NotSupportedException>(() => TestInternal(list, alternateList));
			}
			else
			{
				TestInternal(list, alternateList);
			}
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

		private void TestInternal(IQueryable<T> list, IQueryable<T> alternateList)
		{
			Stopwatch.Start();
			var loadedResult = _query(list);
			Stopwatch.Stop();

			var expectedResult = _query(alternateList);

			Assert.IsTrue(_comparer.Equals(loadedResult, expectedResult), "Query '{0}' is not equal to expected data", _query.Method.Name);
		}
	}
}