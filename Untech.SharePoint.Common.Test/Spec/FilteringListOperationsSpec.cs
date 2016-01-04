using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class FilteringListOperationsSpec : ITestQueryProvider<NewsModel>
	{
		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source.Where(n => n.Description.Contains("Text")).ToList();
		}

		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<NewsModel>.Create(WhereQuery, EntitySequenceComparer.Default)
			};
		}
	}
}
