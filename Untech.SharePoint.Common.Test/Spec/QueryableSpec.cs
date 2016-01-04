using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Spec.DataManagers;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class QueryableSpec : IDisposable
	{
		private readonly IDataContext _dataContext;
		private readonly TestDataManager _dataManager;

		public QueryableSpec(IDataContext dataContext)
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
			Run(_dataContext.News, _dataManager.News, new AggregateListOperationsSpec());
		}

		public void Filtering()
		{
			Run(_dataContext.News, _dataManager.News, new FilteringListOperationsSpec());
		}

		public void Ordering()
		{
			Run(_dataContext.Projects, _dataManager.Projects, new OrderingListOperationsSpec());
		}

		public void Paging()
		{
			Run(_dataContext.News, _dataManager.News, new PagingListOperationsSpec());
		}

		public void Set()
		{
			Run(_dataContext.News, _dataManager.News, new SetListOperationsSpec());
		}

		public void Projection()
		{
			Run(_dataContext.News, _dataManager.News, new ProjectionListOperationsSpec());
		}

		private void Run<T>(ISpList<T> list, IEnumerable<T> alternateList, ITestQueryProvider<T> queryProvider)
		{
			queryProvider.GetTestQueries().Each(n => n.Test(list, alternateList.AsQueryable()));
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_dataManager.Dispose();
		}
	}
}