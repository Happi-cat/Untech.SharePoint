using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class ProjectionQuerySpec : IQueryTestsProvider<NewsModel>
	{
		public IEnumerable<Tuple<string, string>> SelectQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => new Tuple<string, string>(n.Title, n.Description));
		}

		public IEnumerable<string> SelectSelectQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => new Tuple<string, string>(n.Title, n.Description))
				.Select(n => n.Item1);
		}

		public IEnumerable<QueryTest<NewsModel>> GetQueryTests()
		{
			return new[]
			{
				QueryTest<NewsModel>.Functional(SelectQuery),
				QueryTest<NewsModel>.Functional(SelectSelectQuery).Throws<NotSupportedException>()
			};
		}
	}
}
