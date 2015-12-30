using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Spec.Models.Fillers;
using Untech.SharePoint.Common.Test.Spec.Scenarios;
using Untech.SharePoint.Common.Test.Tools;

namespace Untech.SharePoint.Common.Test.Spec
{
	[TestClass]
	public class ProjectionListOperationsTest
	{
		private readonly IDataContext _dataContext;
		private readonly ScenarioRunner _runner;

		public ProjectionListOperationsTest(IDataContext context)
		{
			_runner = new ScenarioRunner();
			_dataContext = context;
		}

		public ProjectionListOperationsTest(IDataContext context, ScenarioRunner runner)
		{
			_runner = runner;
			_dataContext = context;
		}

		public IEnumerable<Tuple<string, string>> SelectQuery(IQueryable<NewsModel> source)
		{
			return source.Select(n => new Tuple<string, string>(n.Title, n.Description));
		}

		[TestMethod]
		public void Select()
		{
			var scenario = Given(SelectQuery, SequenceComparer<Tuple<string,string>>.Default);

			_runner.Run(GetType(), "Select", scenario);
		}

		private FetchScenario<NewsModel, T> Given<T>(Func<IQueryable<NewsModel>, T> query, IEqualityComparer<T> comparer)
		{
			return new FetchScenario<NewsModel, T>(_dataContext.News, query, comparer)
				.WithArray(Fillers.GetNewsFiller(), 100);
		}
	}
}
