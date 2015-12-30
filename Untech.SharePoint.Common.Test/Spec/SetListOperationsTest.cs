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
	/// The set methods are All, Any, Concat, Contains, DefaultIfEmpty, Distinct, EqualAll, Except, Intersect, and Union.
	/// </summary>
	[TestClass]
	public class SetListOperationsTest
	{
		private readonly IDataContext _dataContext;
		private readonly ScenarioRunner _runner;

		public SetListOperationsTest(IDataContext context)
		{
			_runner = new ScenarioRunner();
			_dataContext = context;
		}

		public SetListOperationsTest(IDataContext context, ScenarioRunner runner)
		{
			_runner = runner;
			_dataContext = context;
		}

		public bool AllQuery(IQueryable<NewsModel> source)
		{
			return source.All(n => n.Created > DateTime.Now.AddMonths(-1));
		}


		[TestMethod]
		public void All()
		{
			var scenario = Given(AllQuery, EqualityComparer<bool>.Default);

			_runner.Run(GetType(), "All", scenario);
		}

		public bool AnyQuery(IQueryable<NewsModel> source)
		{
			return source.Any();
		}

		[TestMethod]
		public void Any()
		{
			var scenario = Given(AnyQuery, EqualityComparer<bool>.Default);

			_runner.Run(GetType(), "Any", scenario);
		}

		public bool AnyPQuery(IQueryable<NewsModel> source)
		{
			return source.All(n => n.Description.StartsWith("TODO"));
		}

		[TestMethod]
		public void AnyP()
		{
			var scenario = Given(AnyPQuery, EqualityComparer<bool>.Default);

			_runner.Run(GetType(), "AnyP", scenario);
		}

		private FetchScenario<NewsModel, T> Given<T>(Func<IQueryable<NewsModel>, T> query, IEqualityComparer<T> comparer)
		{
			return new FetchScenario<NewsModel, T>(_dataContext.News, query, comparer)
				.WithArray(Fillers.GetNewsFiller(), 25)
				.WithArray(Fillers.GetNewsFiller().WithStatic(n => n.Description, "TODO"), 25);
		}
	}
}
