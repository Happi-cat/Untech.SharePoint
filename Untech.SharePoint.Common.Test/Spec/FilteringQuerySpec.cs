using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.Comparers;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class FilteringQuerySpec : IQueryTestsProvider<NewsModel>, IQueryTestsProvider<ProjectModel>, IQueryTestsProvider<TeamModel>
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

		public IEnumerable<ProjectModel> WhereLookupNotNull(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Team != null);
		}

		public IEnumerable<ProjectModel> WhereLookupNotEqual(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Team != new ObjectReference { Id = 1});
		}

		public IEnumerable<ProjectModel> WhereLookupEqual(IQueryable<ProjectModel> source)
		{
			var firstTeamRef = source.Where(n => n.Team != null).Select(n => n.Team).First();

			return source
				.Where(n => n.Team == firstTeamRef);
		}

		public IEnumerable<ProjectModel> WhereLookupMultiNotNull(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.SubProjects != null);
		}

		public IEnumerable<ProjectModel> WhereLookupMultiNotContains(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.SubProjects != null && !n.SubProjects.Contains(new ObjectReference { Id = 1 }));
		}

		public IEnumerable<ProjectModel> WhereLookupMultiContains(IQueryable<ProjectModel> source)
		{
			var firstSubprojectRefs = source.Where(n => n.SubProjects != null).Select(n => n.SubProjects).First();

			return source
				.Where(n => n.SubProjects != null && n.SubProjects.Contains(firstSubprojectRefs.First()));
		}

		public IEnumerable<TeamModel> WhereUserNotNull(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.ProjectManager != null);
		}

		public IEnumerable<TeamModel> WhereUserNotEqual(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.ProjectManager != new UserInfo { Id = 1 });
		}

		public IEnumerable<TeamModel> WhereUserEqual(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.ProjectManager == new UserInfo { Id = 1 });
		}

		public IEnumerable<TeamModel> WhereUserMultiNotNull(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.BackendDevelopers != null);
		}

		public IEnumerable<TeamModel> WhereUserMultiNotContains(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.BackendDevelopers != null && !n.BackendDevelopers.Contains(new UserInfo { Id = 1 }));
		}

		public IEnumerable<TeamModel> WhereUserMultiContains(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.BackendDevelopers != null && n.BackendDevelopers.Contains(new UserInfo { Id = 1 }));
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
				QueryTest<ProjectModel>.Functional(WhereCalculatedQuery2, EntityComparer.Default),

				QueryTest<ProjectModel>.Functional(WhereLookupNotNull, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(WhereLookupNotEqual, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(WhereLookupEqual, EntityComparer.Default),

				QueryTest<ProjectModel>.Functional(WhereLookupMultiNotNull, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(WhereLookupMultiNotContains, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(WhereLookupMultiContains, EntityComparer.Default)
			};
		}

		IEnumerable<QueryTest<TeamModel>> IQueryTestsProvider<TeamModel>.GetQueryTests()
		{
			return new[]
			{
				QueryTest<TeamModel>.Functional(WhereUserNotNull, EntityComparer.Default),
				QueryTest<TeamModel>.Functional(WhereUserNotEqual, EntityComparer.Default),
				QueryTest<TeamModel>.Functional(WhereUserEqual, EntityComparer.Default),

				QueryTest<TeamModel>.Functional(WhereUserMultiNotNull, EntityComparer.Default),
				QueryTest<TeamModel>.Functional(WhereUserMultiNotContains, EntityComparer.Default),
				QueryTest<TeamModel>.Functional(WhereUserMultiContains, EntityComparer.Default)
			};
		}
	}
}
