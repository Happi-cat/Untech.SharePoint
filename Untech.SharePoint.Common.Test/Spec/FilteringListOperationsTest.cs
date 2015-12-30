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
	[TestClass]
	public class FilteringListOperationsTest
	{
		private readonly IDataContext _dataContext;
		private readonly ScenarioRunner _runner;

		public FilteringListOperationsTest(IDataContext context)
		{
			_runner = new ScenarioRunner();
			_dataContext = context;
		}

		public FilteringListOperationsTest(IDataContext context, ScenarioRunner runner)
		{
			_runner = runner;
			_dataContext = context;
		}

		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source.Where(n => n.Description.Contains("Text")).ToList();
		} 
		
		[TestMethod]
		public void Where()
		{
			var scenario = Given(WhereQuery, EntitySequenceComparer<NewsModel>.Default);

			_runner.Run(GetType(), "Select", scenario);
		}

		private FetchScenario<NewsModel, T> Given<T>(Func<IQueryable<NewsModel>, T> query, IEqualityComparer<T> comparer)
		{
			return new FetchScenario<NewsModel, T>(_dataContext.News, query, comparer)
				.WithArray(Fillers.GetNewsFiller(), 100)
				.WithArray(Fillers.GetNewsFiller().WithStatic(n => n.Description, "This Description Contains Some Copypasted Text"), 100);
		}
	}
}
