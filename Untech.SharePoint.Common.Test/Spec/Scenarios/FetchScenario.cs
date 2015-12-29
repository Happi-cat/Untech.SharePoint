using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec.Scenarios
{
	public class FetchScenario<T, TResult> : ListScenario<T>
	{
		private readonly List<ArrayGenerator<T>> _itemGenerators = new List<ArrayGenerator<T>>();
		private readonly Func<IQueryable<T>, TResult> _query;
		private readonly IEqualityComparer<TResult> _comparer;
		private IReadOnlyCollection<T> _addedItems;

		public FetchScenario(ISpList<T> list, Func<IQueryable<T>, TResult> query, IEqualityComparer<TResult> comparer)
			: base(list)
		{
			_query = query;
			_comparer = comparer;
		}

		public FetchScenario<T, TResult> WithArray(IValueGenerator<T> item, int size)
		{
			_itemGenerators.Add(new ArrayGenerator<T>(item) {Size = size});
			
			return this;
		}

		public override void BeforeRun()
		{
			_addedItems = _itemGenerators
				.SelectMany(n => n.Generate())
				.Select(n => List.Add(n))
				.ToList();
		}

		public override void Run()
		{
			Stopwatch.Start();
			var loadedResult = _query(List);
			Stopwatch.Stop();

			var expectedResult = _query(_addedItems.AsQueryable());

			Assert.IsTrue(_comparer.Equals(loadedResult, expectedResult));
		}

		public override void AfterRun()
		{
			_addedItems.Each(n => List.Delete(n));
		}
	}
}