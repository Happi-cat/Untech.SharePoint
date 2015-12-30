using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Spec.Models.Fillers;
using Untech.SharePoint.Common.Test.Spec.Scenarios;
using Untech.SharePoint.Common.Test.Tools;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The aggregate methods are Aggregate, Average, Count, LongCount, Max, Min, and Sum.
	/// </summary>
	[TestClass]
	public class AggregateListOperationsTest
	{
		private readonly IDataContext _dataContext;
		private readonly ScenarioRunner _runner;

		public AggregateListOperationsTest(IDataContext context)
		{
			_runner = new ScenarioRunner();
			_dataContext = context;
		}

		public AggregateListOperationsTest(IDataContext context, ScenarioRunner runner)
		{
			_runner = runner;
			_dataContext = context;
		}

		public int CountQuery(IQueryable<Entity> source)
		{
			return source.Count();
		}

		[TestMethod]
		public void Count()
		{
			var scenario = new FetchScenario<NewsModel,int>(_dataContext.News, CountQuery, EqualityComparer<int>.Default)
				.WithArray(Fillers.GetNewsFiller(), 2);

			_runner.Run(GetType(), "Count", scenario);
		}

		public int CountPQuery(IQueryable<NewsModel> source)
		{
			return source.Count(n => n.Description.StartsWith("ABC"));
		}

		[TestMethod]
		public void CountP()
		{
			var scenario = new FetchScenario<NewsModel, int>(_dataContext.News, CountPQuery, EqualityComparer<int>.Default)
				.WithArray(Fillers.GetNewsFiller(), 4)
				.WithArray(Fillers.GetNewsFiller().WithStatic(n => n.Description, "ABC - description"), 2);

			_runner.Run(GetType(), "CountP", scenario);
		}

		public int MinPQuery(IQueryable<Entity> source)
		{
			return source.Min(n => n.Id);
		}

		[TestMethod]
		public void MinP()
		{
			var scenario = new FetchScenario<NewsModel, int>(_dataContext.News, MinPQuery, EqualityComparer<int>.Default)
				.WithArray(Fillers.GetNewsFiller(), 2);

			_runner.Run(GetType(), "Count", scenario);
		}

		public int MaxPQuery(IQueryable<Entity> source)
		{
			return source.Max(n => n.Id);
		}

		[TestMethod]
		public void MaxP()
		{
			var scenario = new FetchScenario<NewsModel, int>(_dataContext.News, MaxPQuery, EqualityComparer<int>.Default)
				.WithArray(Fillers.GetNewsFiller(), 2);

			_runner.Run(GetType(), "Count", scenario);
		}
	}
}
