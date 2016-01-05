using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class FilteringListOperationsSpec : ITestQueryProvider<NewsModel>
	{
		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.ToList();
		}

		public IEnumerable<NewsModel> WhereTake10Query(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Take(10)
				.ToList();
		}

		public IEnumerable<NewsModel> Take10WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.ToList();
		}

		public IEnumerable<NewsModel> WhereWhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => n.Created > DateTime.Now.AddMonths(-1) && n.Title.Contains("lorem"))
				.ToList();
		}

		public IEnumerable<NewsModel> WhereWhereTrueQuery(IQueryable<NewsModel> source)
		{
			var flag = true;
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => flag)
				.ToList();
		}

		public IEnumerable<NewsModel> WhereWhereFalseQuery(IQueryable<NewsModel> source)
		{
			var flag = false;
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => flag)
				.ToList();
		}

		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<NewsModel>.Create(WhereQuery, EntitySequenceComparer.Default),

				TestQuery<NewsModel>.Create(WhereTake10Query, EntitySequenceComparer.Default),
				TestQuery<NewsModel>.Create(Take10WhereQuery).Throws<NotSupportedException>(),

				TestQuery<NewsModel>.Create(WhereWhereQuery, EntitySequenceComparer.Default),
				TestQuery<NewsModel>.Create(WhereWhereTrueQuery, EntitySequenceComparer.Default),
				TestQuery<NewsModel>.Create(WhereWhereFalseQuery, EntitySequenceComparer.Default)
			};
		}
	}
}
