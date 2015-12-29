using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Spec.Models.Fillers;
using Untech.SharePoint.Common.Test.Spec.Scenarios;
using Untech.SharePoint.Common.Test.Tools;

namespace Untech.SharePoint.Common.Test.Spec
{
	[TestClass]
	public class FetchListOperationsTest
	{
		private readonly IDataContext _dataContext;
		private readonly ScenarioRunner _runner;

		public FetchListOperationsTest(IDataContext context)
		{
			_runner = new ScenarioRunner();
			_dataContext = context;
		}

		public FetchListOperationsTest(IDataContext context, ScenarioRunner runner)
		{
			_runner = runner;
			_dataContext = context;
		}

		[TestMethod]
		public void Count()
		{
			var scenario = Given<NewsModel, int>()
				.UseList(_dataContext.News)
				.UseQuery(q => q.Count())
				.Get()
				.WithArray(Fillers.GetNewsFiller(), 2);

			_runner.Run(GetType(), "Count", scenario);
		}

		[TestMethod]
		public void MinP()
		{
			var scenario = Given<NewsModel, int>()
				.UseList(_dataContext.News)
				.UseQuery(q => q.Min(n => n.Id))
				.Get()
				.WithArray(Fillers.GetNewsFiller(), 2);

			_runner.Run(GetType(), "Count", scenario);
		}

		[TestMethod]
		public void MaxP()
		{
			var scenario = Given<NewsModel, int>()
				.UseList(_dataContext.News)
				.UseQuery(q => q.Max(n => n.Id))
				.Get()
				.WithArray(Fillers.GetNewsFiller(), 2);

			_runner.Run(GetType(), "Count", scenario);
		}

		private ScenarioBuilder<T, TResult> Given<T, TResult>()
		{
			return new ScenarioBuilder<T, TResult>();
		}

		private class ScenarioBuilder<T, TResult>
		{
			private ISpList<T> _list;
			private Func<IQueryable<T>, TResult> _query;
			private IEqualityComparer<TResult> _comparer;

			public ScenarioBuilder<T, TResult> UseList(ISpList<T> list)
			{
				_list = list;	
				return this;
			}

			public ScenarioBuilder<T, TResult> UseQuery(Func<IQueryable<T>, TResult> query)
			{
				_query = query;
				return this;
			}

			public ScenarioBuilder<T, TResult> UseComparer(IEqualityComparer<TResult> comparer)
			{
				_comparer = comparer;
				return this;
			}

			public FetchScenario<T, TResult> Get()
			{
				return new FetchScenario<T, TResult>(_list, _query, _comparer ?? EqualityComparer<TResult>.Default);
			}
		}
	}
}
