using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class PerfQuerySpec : IQueryTestsProvider<NewsModel>
	{
		public IEnumerable<QueryTest<NewsModel>> GetQueryTests()
		{
			throw new NotImplementedException();
			//return new[]
			//{
			//	//QueryTest<NewsModel>.Create(), 
			//};
		}

		public IEnumerable<NewsModel> FetchAll(IQueryable<NewsModel> source)
		{
			return source;
		}

		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source;
		}
	}
}