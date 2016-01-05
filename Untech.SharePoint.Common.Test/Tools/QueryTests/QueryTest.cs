using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Test.Tools.Comparers;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public abstract class QueryTest<T>
	{
		#region [Static]

		public static QueryTest<T> Create<TResult>(Func<IQueryable<T>, TResult> query)
		{
			return new QueryObjectTest<T,TResult>(query, EqualityComparer<TResult>.Default);
		}

		public static QueryTest<T> Create<TResult>(Func<IQueryable<T>, IEnumerable<TResult>> query)
		{
			return new QuerySequenceTest<T,TResult>(query, SequenceComparer<TResult>.Default);
		}

		public static QueryTest<T> Create<TResult>(Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
		{
			return new QueryObjectTest<T, TResult>(query, comparer);
		}

		public static QueryTest<T> Create<TResult>(Func<IQueryable<T>, IEnumerable<TResult>> query,
			IEqualityComparer<IEnumerable<TResult>> comparer)
		{
			return new QuerySequenceTest<T,TResult>(query, comparer);
		}

		#endregion


		public abstract void Test(ISpList<T> list, IQueryable<T> alternateList);

		public abstract TimeSpan GetElapsedTime();

		public abstract int GetItemsCounter();

		public QueryTest<T> Throws<TException>()
			where TException : Exception
		{
			return new QueryTestExceptionWrapper<T, TException>(this);
		}
	}

	public class QueryObjectTest<T, TResult> : QueryTest<T>
	{
		private readonly Func<IQueryable<T>, TResult> _query;
		private readonly IEqualityComparer<TResult> _comparer;
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private int _itemsCounter;

		public QueryObjectTest(Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
		{
			_query = query;
			_comparer = comparer;
		}

		public override void Test(ISpList<T> list, IQueryable<T> alternateList)
		{
			try
			{
				var result = _comparer.Equals(GetResult(list), GetExpectedResult(alternateList));
				Assert.IsTrue(result, "Query '{0}' is not equal to expected data", this);
			}
			catch (Exception e)
			{
				e.Data["Query"] = _query;
				e.Data["QueryName"] = _query.Method.Name;

				throw;
			}
		}

		public override TimeSpan GetElapsedTime()
		{
			return _stopwatch.Elapsed;
		}

		public override int GetItemsCounter()
		{
			return _itemsCounter;
		}

		protected virtual TResult GetResult(ISpList<T> list)
		{
			_stopwatch.Start();
			var loadedResult = _query(list);
			_stopwatch.Stop();

			_itemsCounter++;

			return loadedResult;
		}

		protected virtual TResult GetExpectedResult(IQueryable<T> alternateList)
		{
			return _query(alternateList);
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

	public class QuerySequenceTest<T, TResult> : QueryTest<T>
	{
		private readonly Func<IQueryable<T>, IEnumerable<TResult>> _query;
		private readonly IEqualityComparer<IEnumerable<TResult>> _comparer;
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private int _itemsCounter;

		public QuerySequenceTest(Func<IQueryable<T>, IEnumerable<TResult>> query, IEqualityComparer<IEnumerable<TResult>> comparer)
		{
			_query = query;
			_comparer = comparer;
		}

		public override void Test(ISpList<T> list, IQueryable<T> alternateList)
		{
			try
			{
				var result = _comparer.Equals(GetResult(list), GetExpectedResult(alternateList));
				Assert.IsTrue(result, "Query '{0}' is not equal to expected data", this);
			}
			catch (Exception e)
			{
				e.Data["Query"] = _query;
				e.Data["QueryName"] = _query.Method.Name;

				throw;
			}
		}

		public override TimeSpan GetElapsedTime()
		{
			return _stopwatch.Elapsed;
		}

		public override int GetItemsCounter()
		{
			return _itemsCounter;
		}

		protected virtual IEnumerable<TResult> GetResult(ISpList<T> list)
		{
			_stopwatch.Start();
			var loadedResult = _query(list).ToList();
			_stopwatch.Stop();

			_itemsCounter += loadedResult.Count;

			return loadedResult;
		}

		protected virtual IEnumerable<TResult> GetExpectedResult(IQueryable<T> alternateList)
		{
			return _query(alternateList).ToList();
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