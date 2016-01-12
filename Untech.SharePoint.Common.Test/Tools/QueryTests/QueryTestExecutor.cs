using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class QueryTestExecutor<T>
	{
		public const int Attempts = 1000;

		public QueryTestExecutor()
		{
			LinqQueryFetchTimer = new Stopwatch();
			CamlQueryFetchTimer = new Stopwatch();
		}

		public ISpList<T> List { get; set; }

		public IQueryable<T> AlternateList { get; set; }

		public Stopwatch LinqQueryFetchTimer { get; set; }

		public Stopwatch CamlQueryFetchTimer { get; set; }

		public int ItemsCounter { get; set; }

		public string FilePath { get; set; }

		public static QueryTestExecutor<T> Functional(ISpList<T> list, IQueryable<T> alternateList)
		{
			return new QueryTestExecutor<T>
			{
				List = list,
				AlternateList = alternateList
			};
		}

		public void Execute(QueryTest<T> queryTest)
		{
			queryTest.Accept(this);
		}

		public void Execute<TException>(ExceptionQueryTest<T, TException> test)
			where TException : Exception
		{
			CustomAssert.Throw<TException>(() => Execute(test.Inner));
		}

		public void Execute<TResult>(ObjectQueryTest<T, TResult> test)
		{
			try
			{
				var actual = test.Query(List);
				var expected = test.Query(AlternateList);

				var result = test.Comparer.Equals(actual, expected);
				Assert.IsTrue(result, "Query '{0}' is not equal to expected data", this);
			}
			catch (Exception e)
			{
				e.Data["Query"] = test.Query;
				e.Data["QueryName"] = test.Query.Method.Name;

				throw;
			}
		}

		public void Execute<TResult>(SequenceQueryTest<T, TResult> test)
		{
			try
			{
				var actual = test.Query(List).ToList();
				var expected = test.Query(AlternateList).ToList();

				var result = test.Comparer.Equals(actual, expected);
				Assert.IsTrue(result, "Query '{0}' is not equal to expected data", this);
			}
			catch (Exception e)
			{
				e.Data["Query"] = test.Query;
				e.Data["QueryName"] = test.Query.Method.Name;

				throw;
			}
		}

		public void Execute<TResult>(ObjectQueryPerfTest<T, TResult> test)
		{
			ItemsCounter = 0;
			LinqQueryFetchTimer.Reset();
			CamlQueryFetchTimer.Reset();

			var counter = Attempts;
			while (counter-- > 0)
			{
				var result = MeasureQuery(test.Query);

				Assert.IsNotNull(result);
				ItemsCounter++;

				MeasureCaml(test.Caml);
			}

			LogResult(test.Query.Method, ItemsCounter, LinqQueryFetchTimer.Elapsed, CamlQueryFetchTimer.Elapsed);
		}

		public void Execute<TResult>(SequenceQueryPerfTest<T, TResult> test)
		{
			ItemsCounter = 0;
			LinqQueryFetchTimer.Reset();
			CamlQueryFetchTimer.Reset();

			var counter = Attempts;
			while (counter-- > 0)
			{
				var result = MeasureQuery(test.Query);

				Assert.IsNotNull(result);
				ItemsCounter += result.Count;

				MeasureCaml(test.Caml);
			}

			LogResult(test.Query.Method, ItemsCounter, LinqQueryFetchTimer.Elapsed, CamlQueryFetchTimer.Elapsed);
		}

		public TResult MeasureQuery<TResult>(Func<IQueryable<T>, TResult> query)
		{
			LinqQueryFetchTimer.Start();
			var result = query(List);
			LinqQueryFetchTimer.Stop();

			return result;
		}

		public List<TResult> MeasureQuery<TResult>(Func<IQueryable<T>, IEnumerable<TResult>> query)
		{
			LinqQueryFetchTimer.Start();
			var result = query(List).ToList();
			LinqQueryFetchTimer.Stop();

			return result;
		}

		public virtual void MeasureCaml(string caml)
		{

		}

		private void LogResult(MethodInfo method, int itemsCount, TimeSpan elapsedLinqTime, TimeSpan elapsedCamlTime)
		{
			var category = "";
			if (method.DeclaringType != null)
			{
				category = method.DeclaringType.Name;
			}
			EnsureFileExists();
			using (var file = File.AppendText(FilePath))
			{
				file.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",
					category, method.Name + "-Linq", Attempts, itemsCount, elapsedLinqTime.Ticks,
					elapsedLinqTime, new TimeSpan(elapsedLinqTime.Ticks / Attempts), itemsCount / Attempts);

				file.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",
					category, method.Name + "-Caml", Attempts, itemsCount, elapsedCamlTime.Ticks,
					elapsedCamlTime, new TimeSpan(elapsedCamlTime.Ticks / Attempts), itemsCount / Attempts);
			}
		}

		private void EnsureFileExists()
		{
			if (File.Exists(FilePath))
			{
				return;
			}

			using (var file = File.CreateText(FilePath))
			{
				file.WriteLine("Category;Query;Attempts;Items;Ticks;Timespan;TimespanPerAttempt;ItemsPerAttempt");
			}
		}
	}
}