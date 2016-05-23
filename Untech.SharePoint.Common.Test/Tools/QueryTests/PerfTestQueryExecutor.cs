using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	public class PerfTestQueryExecutor<T> : ITestQueryExcecutor<T>
	{
		public const int Attempts = 1000;

		public PerfTestQueryExecutor()
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

		public void Visit<TResult>(Func<IQueryable<T>, object> query, IEqualityComparer<TResult> comparer, Type exception, string caml)
		{
			if (typeof (TResult).IsIEnumerable())
			{
				ExecuteSequence(query, caml);
			}
			else
			{
				ExecuteSingle(query, caml);
			}
		}

		public void ExecuteSingle(Func<IQueryable<T>, object> query, string caml)
		{
			ItemsCounter = 0;
			LinqQueryFetchTimer.Reset();
			CamlQueryFetchTimer.Reset();

			var counter = Attempts;
			while (counter-- > 0)
			{
				var result = MeasureSingle(query);

				Assert.IsNotNull(result);
				ItemsCounter++;

				MeasureCaml(caml);
			}

			LogResult(query.Method, ItemsCounter, LinqQueryFetchTimer.Elapsed, CamlQueryFetchTimer.Elapsed);
		}

		public void ExecuteSequence(Func<IQueryable<T>, object> query, string caml)
		{
			ItemsCounter = 0;
			LinqQueryFetchTimer.Reset();
			CamlQueryFetchTimer.Reset();

			var counter = Attempts;
			while (counter-- > 0)
			{
				var result = MeasureSequence(query);

				Assert.IsNotNull(result);
				ItemsCounter += result.Count;

				MeasureCaml(caml);
			}

			LogResult(query.Method, ItemsCounter, LinqQueryFetchTimer.Elapsed, CamlQueryFetchTimer.Elapsed);
		}

		public object MeasureSingle(Func<IQueryable<T>, object> query)
		{
			LinqQueryFetchTimer.Start();
			var result = query(List);
			LinqQueryFetchTimer.Stop();

			return result;
		}

		public List<object> MeasureSequence(Func<IQueryable<T>, object> query)
		{
			var list = new List<object>();

			LinqQueryFetchTimer.Start();
			var enumerator = ((IEnumerable) query(List)).GetEnumerator();
			while (enumerator.MoveNext())
			{
				list.Add(enumerator.Current);
			}
			LinqQueryFetchTimer.Stop();

			return list;
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