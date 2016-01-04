using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class ProjectionListOperationsSpec : ITestQueryProvider<NewsModel>
	{
		public IEnumerable<Tuple<string, string>> SelectQuery(IQueryable<NewsModel> source)
		{
			return source.Select(n => new Tuple<string, string>(n.Title, n.Description));
		}

		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<NewsModel>.Create(SelectQuery, SequenceComparer<Tuple<string,string>>.Default)
			};
		}
	}
}
