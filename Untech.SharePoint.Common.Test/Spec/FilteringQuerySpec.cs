using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.Comparers;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class FilteringQuerySpec : IQueryTestsProvider<NewsModel>, IQueryTestsProvider<ProjectModel>
	{
		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"));
		}

		public IEnumerable<ProjectModel> WhereQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Status.In(new [] { "Approved", "Cancelled" }) && n.Technology == "Java")
				.Where(n => n.OSes != null && n.OSes.Contains("Linux"));
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

		public IEnumerable<ProjectModel> WhereCalculatedQuery1(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Over10Days);
		}

		public IEnumerable<ProjectModel> WhereCalculatedQuery2(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.ProjectLaunch > DateTime.Now.AddMonths(-12));
		}

		public IEnumerable<QueryTest<NewsModel>> GetQueryTests()
		{
			return new[]
			{
				QueryTest<NewsModel>.Functional(WhereQuery, EntityComparer.Default),

				QueryTest<NewsModel>.Functional(WhereTake10Query, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(Take10WhereQuery).Throws<NotSupportedException>(),

				QueryTest<NewsModel>.Functional(WhereWhereQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(WhereWhereTrueQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(WhereWhereFalseQuery, EntityComparer.Default)
			};
		}

		IEnumerable<QueryTest<ProjectModel>> IQueryTestsProvider<ProjectModel>.GetQueryTests()
		{
			return new[]
			{
				QueryTest<ProjectModel>.Functional(WhereQuery, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(WhereCalculatedQuery1, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(WhereCalculatedQuery2, EntityComparer.Default)
			};
		}
	}
}
