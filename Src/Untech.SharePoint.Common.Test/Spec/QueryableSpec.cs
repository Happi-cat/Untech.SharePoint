using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Spec.Models;
using Untech.SharePoint.Common.TestTools.DataManagers;
using Untech.SharePoint.Common.TestTools.QueryTests;

namespace Untech.SharePoint.Common.Spec
{
	public class QueryableSpec
	{
		private readonly DataContext _dataContext;
		private readonly TestDataManager _dataManager;

		public QueryableSpec(DataContext dataContext)
		{
			_dataContext = dataContext;
			_dataManager = new TestDataManager(_dataContext);
		}

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
			Run(_dataContext.Projects, _dataManager.Projects, new FilteringQuerySpec());
			Run(_dataContext.Teams, _dataManager.Teams, new FilteringQuerySpec());
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

		private void Run<T>(ISpList<T> list, IReadOnlyList<T> alternateList, ITestQueryProvider<T> queryProvider)
		{
			var executor = new SimpleTestQueryExecutor<T> { List = list, AlternateList = alternateList.AsQueryable() };
			foreach (var query in queryProvider.GetQueries())
			{
				((TestQueryBuilder<T>)query).Accept(executor);
			}
		}
	}
}