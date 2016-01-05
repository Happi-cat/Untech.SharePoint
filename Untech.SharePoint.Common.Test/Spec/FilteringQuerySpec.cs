using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.Comparers;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class FilteringQuerySpec : IQueryTestsProvider<NewsModel>
	{
		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"));
		}

		public IEnumerable<NewsModel> WhereTake10Query(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Take(10);
		}

		public IEnumerable<NewsModel> Take10WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Where(n => n.Description.StartsWith("DESCRIPTION"));
		}

		public IEnumerable<NewsModel> WhereWhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => n.Created > DateTime.Now.AddMonths(-1) && n.Title.Contains("lorem"));
		}

		public IEnumerable<NewsModel> WhereWhereTrueQuery(IQueryable<NewsModel> source)
		{
			var flag = true;
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => flag);
		}

		public IEnumerable<NewsModel> WhereWhereFalseQuery(IQueryable<NewsModel> source)
		{
			var flag = false;
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => flag);
		}

		public IEnumerable<QueryTest<NewsModel>> GetQueryTests()
		{
			return new[]
			{
				QueryTest<NewsModel>.Create(WhereQuery, EntityComparer.Default),

				QueryTest<NewsModel>.Create(WhereTake10Query, EntityComparer.Default),
				QueryTest<NewsModel>.Create(Take10WhereQuery).Throws<NotSupportedException>(),

				QueryTest<NewsModel>.Create(WhereWhereQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Create(WhereWhereTrueQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Create(WhereWhereFalseQuery, EntityComparer.Default)
			};
		}
	}
}
