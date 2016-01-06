using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.DataManagers;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class QueryableSpec
	{
		private readonly IDataContext _dataContext;
		private readonly TestDataManager _dataManager;

		public QueryableSpec(IDataContext dataContext)
		{
			_dataContext = dataContext;
			_dataManager = new TestDataManager(_dataContext);
		}

		public string FilePath { get; set; }

		public void Init()
		{
			_dataManager.Init();
		}

		public void Aggregate()
		{
			Run(_dataContext.News, _dataManager.News, new AggregateQuerySpec());
		}

		public void Filtering()
		{
			Run(_dataContext.News, _dataManager.News, new FilteringQuerySpec());
		}

		public void Ordering()
		{
			Run(_dataContext.Projects, _dataManager.Projects, new OrderingQuerySpec());
		}

		public void Paging()
		{
			Run(_dataContext.News, _dataManager.News, new PagingQuerySpec());
		}

		public void Set()
		{
			Run(_dataContext.News, _dataManager.News, new SetQuerySpec());
		}

		public void Projection()
		{
			Run(_dataContext.News, _dataManager.News, new ProjectionQuerySpec());
		}

		private void Run<T>(ISpList<T> list, IReadOnlyList<T> alternateList, IQueryTestsProvider<T> queryProvider)
		{
			var category = queryProvider.GetType().Name;
			foreach (var queryTest in queryProvider.GetQueryTests())
			{
				Run(category, queryTest, list, alternateList);
			}
		}

		private void Run<T>(string category, QueryTest<T> test, ISpList<T> list, IEnumerable<T> alternateList)
		{
			if (string.IsNullOrEmpty(FilePath))
			{
				test.Test(list, alternateList.AsQueryable());
			}
			else
			{
				new QueryTestPerfMeter<T>(FilePath, category, test).Test(list, alternateList.AsQueryable());
			}
		}
	}
}