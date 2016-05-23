using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class ProjectionQuerySpec : ITestQueryProvider<NewsModel>
	{
		public IEnumerable<Tuple<string, string>> SelectQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => new Tuple<string, string>(n.Title, n.Description));
		}

		[QueryException(typeof(NotSupportedException))]
		public IEnumerable<string> SelectSelectQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => new Tuple<string, string>(n.Title, n.Description))
				.Select(n => n.Item1);
		}

		public IEnumerable<Func<IQueryable<NewsModel>, object>> GetQueries()
		{
			return new Func<IQueryable<NewsModel>, object>[]
			{
				SelectQuery,
				SelectSelectQuery
			};
		}
	}
}
