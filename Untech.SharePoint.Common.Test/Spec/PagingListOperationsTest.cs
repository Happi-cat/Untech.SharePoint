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
	/// Paging operations return a single, specific element from a sequence. The element methods are ElementAt, First, FirstOrDefault, Last, LastOrDefault, Single, Skip, Take, TakeWhile.
	/// </summary>
	[TestClass]
	public class PagingListOperationsTest
	{
		private readonly IDataContext _dataContext;
		private readonly ScenarioRunner _runner;

		public PagingListOperationsTest(IDataContext context)
		{
			_runner = new ScenarioRunner();
			_dataContext = context;
		}

		public PagingListOperationsTest(IDataContext context, ScenarioRunner runner)
		{
			_runner = runner;
			_dataContext = context;
		}

		public NewsModel SingleQuery(IQueryable<NewsModel> source)
		{
			return source.Single();
		}

		[TestMethod]
		public void Single()
		{
			var scenario = Given(SingleQuery, EntityComparer.Default);

			_runner.Run(GetType(), "Single", scenario);
		}

		public NewsModel SinglePQuery(IQueryable<NewsModel> source)
		{
			return source.Single(n => n.Description == "SINGLE DESCRIPTION");
		}

		[TestMethod]
		public void SingleP()
		{
			var scenario = Given(SinglePQuery, EntityComparer.Default);

			_runner.Run(GetType(), "SingleP", scenario);
		}

		public NewsModel SingleOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.SingleOrDefault();
		}

		[TestMethod]
		public void SingleOrDefault()
		{
			var scenario = Given(SingleOrDefaultQuery, EntityComparer.Default);

			_runner.Run(GetType(), "SingleOrDefault", scenario);
		}

		public NewsModel SingleOrDefaultPQuery(IQueryable<NewsModel> source)
		{
			return source.Single(n => n.Description == "UNKOWN DESCRIPTION");
		}

		[TestMethod]
		public void SingleOrDefaultP()
		{
			var scenario = Given(SingleOrDefaultPQuery, EntityComparer.Default);

			_runner.Run(GetType(), "SingleOrDefaultP", scenario);
		}

		public NewsModel ElementAtQuery(IQueryable<NewsModel> source)
		{
			return source.ElementAt(2);
		}

		[TestMethod]
		public void ElementAt()
		{
			var scenario = Given(ElementAtQuery, EntityComparer.Default);

			_runner.Run(GetType(), "ElementAt", scenario);
		}

		public NewsModel ElementAtOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.ElementAtOrDefault(100);
		}

		[TestMethod]
		public void ElementAtOrDefault()
		{
			var scenario = Given(ElementAtOrDefaultQuery, EntityComparer.Default);

			_runner.Run(GetType(), "ElementAtOrDefault", scenario);
		}


		public NewsModel FirstQuery(IQueryable<NewsModel> source)
		{
			return source.First();
		}

		[TestMethod]
		public void First()
		{
			var scenario = Given(FirstQuery, EntityComparer.Default);

			_runner.Run(GetType(), "First", scenario);
		}

		public NewsModel FirstOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.FirstOrDefault();
		}

		[TestMethod]
		public void FirstOrDefault()
		{
			var scenario = new FetchScenario<NewsModel, NewsModel>(_dataContext.News, FirstOrDefaultQuery, EntityComparer.Default);

			_runner.Run(GetType(), "FirstOrDefault", scenario);
		}

		public NewsModel FirstPQuery(IQueryable<NewsModel> source)
		{
			return source.First(n => n.Description == "MULTIPLE DESCRIPTION");
		}

		[TestMethod]
		public void FirstP()
		{
			var scenario = Given(FirstPQuery, EntityComparer.Default);

			_runner.Run(GetType(), "FirstP", scenario);
		}

		public NewsModel FirstPOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.FirstOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		[TestMethod]
		public void FirstPOrDefault()
		{
			var scenario = Given(FirstPOrDefaultQuery, EntityComparer.Default);

			_runner.Run(GetType(), "FirstPOrDefault", scenario);
		}

		public NewsModel LastQuery(IQueryable<NewsModel> source)
		{
			return source.Last();
		}

		[TestMethod]
		public void Last()
		{
			var scenario = Given(LastQuery, EntityComparer.Default);

			_runner.Run(GetType(), "Last", scenario);
		}

		public NewsModel LastOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.LastOrDefault();
		}

		[TestMethod]
		public void LastOrDefault()
		{
			var scenario = new FetchScenario<NewsModel, NewsModel>(_dataContext.News, LastOrDefaultQuery, EntityComparer.Default);

			_runner.Run(GetType(), "LastOrDefault", scenario);
		}

		public NewsModel LastPQuery(IQueryable<NewsModel> source)
		{
			return source.Last(n => n.Description == "MULTIPLE DESCRIPTION");
		}

		[TestMethod]
		public void LastP()
		{
			var scenario = Given(LastPQuery, EntityComparer.Default);

			_runner.Run(GetType(), "LastP", scenario);
		}

		public NewsModel LastPOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.LastOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		[TestMethod]
		public void LastPOrDefault()
		{
			var scenario = Given(LastPOrDefaultQuery, EntityComparer.Default);

			_runner.Run(GetType(), "LastPOrDefault", scenario);
		}

		public IEnumerable<NewsModel> TakeQuery(IQueryable<NewsModel> source)
		{
			return source.Take(2).ToList();
		}

		[TestMethod]
		public void Take()
		{
			var scenario = Given(TakeQuery, EntitySequenceComparer<NewsModel>.Default);

			_runner.Run(GetType(), "Take", scenario);
		}

		private FetchScenario<NewsModel, T> Given<T>(Func<IQueryable<NewsModel>, T> query, IEqualityComparer<T> comparer)
		{
			return new FetchScenario<NewsModel, T>(_dataContext.News, query, comparer)
				.WithArray(Fillers.GetNewsFiller(), 4)
				.WithArray(Fillers.GetNewsFiller().WithStatic(n => n.Description, "SIGNLE DESCRIPTION"), 1)
				.WithArray(Fillers.GetNewsFiller().WithStatic(n => n.Description, "MULTIPLE DESCRIPTION"), 3); 
		}
	}
}
