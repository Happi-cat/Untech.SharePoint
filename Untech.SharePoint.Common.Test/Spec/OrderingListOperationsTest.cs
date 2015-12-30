using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Spec.Models.Fillers;
using Untech.SharePoint.Common.Test.Spec.Scenarios;
using Untech.SharePoint.Common.Test.Tools;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The ordering methods are OrderBy, OrderByDescending, ThenBy, ThenByDescending, and Reverse.
	/// </summary>
	[TestClass]
	public class OrderingListOperationsTest
	{
		private readonly IDataContext _dataContext;
		private readonly ScenarioRunner _runner;

		public OrderingListOperationsTest(IDataContext context)
		{
			_runner = new ScenarioRunner();
			_dataContext = context;
		}

		public OrderingListOperationsTest(IDataContext context, ScenarioRunner runner)
		{
			_runner = runner;
			_dataContext = context;
		}

		public IEnumerable<NewsModel> OrderByQuery(IQueryable<NewsModel> source)
		{
			return source.OrderBy(n => n.Title).ToList();
		}
		
		[TestMethod]
		public void OrderBy()
		{
			var scenario = Given(OrderByQuery, EntitySequenceComparer<NewsModel>.Default);

			_runner.Run(GetType(), "OrderBy", scenario);
		}

		public IEnumerable<NewsModel> OrderByDescQuery(IQueryable<NewsModel> source)
		{
			return source.OrderByDescending(n => n.Title).ToList();
		}

		[TestMethod]
		public void OrderByDesc()
		{
			var scenario = Given(OrderByDescQuery, EntitySequenceComparer<NewsModel>.Default);

			_runner.Run(GetType(), "OrderBy", scenario);
		}


		public IEnumerable<NewsModel> ThenByQuery(IQueryable<NewsModel> source)
		{
			return source.OrderBy(n => n.Description).ThenBy(n => n.Title).ToList();
		}

		[TestMethod]
		public void ThenBy()
		{
			var scenario = Given(ThenByQuery, EntitySequenceComparer<NewsModel>.Default);

			_runner.Run(GetType(), "ThenBy", scenario);
		}

		public IEnumerable<NewsModel> ThenByDescQuery(IQueryable<NewsModel> source)
		{
			return source.OrderBy(n => n.Description).ThenByDescending(n => n.Title).ToList();
		}

		[TestMethod]
		public void ThenByDesc()
		{
			var scenario = Given(ThenByDescQuery, EntitySequenceComparer<NewsModel>.Default);

			_runner.Run(GetType(), "ThenBy", scenario);
		}

		public IEnumerable<NewsModel> ReverseQuery(IQueryable<NewsModel> source)
		{
			return source.OrderBy(n => n.Description).ThenBy(n => n.Title).Reverse().ToList();
		}

		[TestMethod]
		public void Reverse()
		{
			var scenario = Given(ReverseQuery, EntitySequenceComparer<NewsModel>.Default);

			_runner.Run(GetType(), "Reverse", scenario);
		}


		private FetchScenario<NewsModel, T> Given<T>(Func<IQueryable<NewsModel>, T> query, IEqualityComparer<T> comparer)
		{
			return new FetchScenario<NewsModel, T>(_dataContext.News, query, comparer)
				.WithArray(Fillers.GetNewsFiller(), 25)
				.WithArray(Fillers.GetNewsFiller().WithStatic(n => n.Description, "Identical #1"), 25)
				.WithArray(Fillers.GetNewsFiller().WithStatic(n => n.Description, "Identical #2"), 25)
				.WithArray(Fillers.GetNewsFiller().WithStatic(n => n.Description, "Identical #3"), 25);
		}
	}
}
