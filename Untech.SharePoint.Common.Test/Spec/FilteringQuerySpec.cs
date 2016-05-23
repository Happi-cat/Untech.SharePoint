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
	public class FilteringQuerySpec : ITestQueryProvider<NewsModel>, ITestQueryProvider<ProjectModel>,
		ITestQueryProvider<TeamModel>
	{
		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<NewsModel> WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"));
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<ProjectModel> WhereQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Status.In(new[] {"Approved", "Cancelled"}) && n.Technology == "Java")
				.Where(n => n.OSes != null && n.OSes.Contains("Linux"));
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<NewsModel> WhereTake10Query(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Take(10);
		}

		[QueryException(typeof (NotSupportedException))]
		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<NewsModel> Take10WhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Where(n => n.Description.StartsWith("DESCRIPTION"));
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<NewsModel> WhereWhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => n.Created > DateTime.Now.AddMonths(-1) && n.Title.Contains("lorem"));
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<NewsModel> WhereWhereTrueQuery(IQueryable<NewsModel> source)
		{
			var flag = true;
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => flag);
		}

		[QueryComparer(typeof (EntityComparer))]
		[QueryException(typeof (NotSupportedException))]
		public IEnumerable<NewsModel> WhereWhereFalseQuery(IQueryable<NewsModel> source)
		{
			var flag = false;
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Where(n => flag);
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<ProjectModel> WhereCalculatedQuery1(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Over10Days);
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<ProjectModel> WhereCalculatedQuery2(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.ProjectLaunch > DateTime.Now.AddMonths(-12));
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<ProjectModel> WhereLookupNotNull(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Team != null);
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<ProjectModel> WhereLookupNotEqual(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Team != new ObjectReference {Id = 1});
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<ProjectModel> WhereLookupEqual(IQueryable<ProjectModel> source)
		{
			var firstTeamRef = source.Where(n => n.Team != null).Select(n => n.Team).First();

			return source
				.Where(n => n.Team == firstTeamRef);
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<ProjectModel> WhereLookupMultiNotNull(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.SubProjects != null);
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<ProjectModel> WhereLookupMultiNotContains(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.SubProjects != null && !n.SubProjects.Contains(new ObjectReference {Id = 1}));
		}

		[QueryComparer(typeof (EntityComparer))]
		[QueryException(typeof (NotSupportedException))]
		public IEnumerable<ProjectModel> WhereLookupMultiContains(IQueryable<ProjectModel> source)
		{
			var firstSubprojectRefs = source.Where(n => n.SubProjects != null).Select(n => n.SubProjects).First();

			return source
				.Where(n => n.SubProjects != null && n.SubProjects.Contains(firstSubprojectRefs.First()));
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<TeamModel> WhereUserNotNull(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.ProjectManager != null);
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<TeamModel> WhereUserNotEqual(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.ProjectManager != new UserInfo {Id = 1});
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<TeamModel> WhereUserEqual(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.ProjectManager == new UserInfo {Id = 1});
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<TeamModel> WhereUserMultiNotNull(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.BackendDevelopers != null);
		}

		[QueryComparer(typeof (EntityComparer))]
		public IEnumerable<TeamModel> WhereUserMultiNotContains(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.BackendDevelopers != null && !n.BackendDevelopers.Contains(new UserInfo {Id = 1}));
		}

		[QueryComparer(typeof (EntityComparer))]
		[QueryException(typeof (NotSupportedException))]
		public IEnumerable<TeamModel> WhereUserMultiContains(IQueryable<TeamModel> source)
		{
			return source
				.Where(n => n.BackendDevelopers != null && n.BackendDevelopers.Contains(new UserInfo {Id = 1}));
		}

		public IEnumerable<Func<IQueryable<NewsModel>, object>> GetQueries()
		{
			return new Func<IQueryable<NewsModel>, object>[]
			{
				WhereQuery,

				WhereTake10Query,
				Take10WhereQuery,

				WhereWhereQuery,
				WhereWhereTrueQuery,
				WhereWhereFalseQuery,
			};
		}

		IEnumerable<Func<IQueryable<ProjectModel>, object>> ITestQueryProvider<ProjectModel>.GetQueries()
		{
			return new Func<IQueryable<ProjectModel>, object>[]
			{
				WhereQuery,
				WhereCalculatedQuery1,
				WhereCalculatedQuery2,

				WhereLookupNotNull,
				WhereLookupNotEqual,
				WhereLookupEqual,

				WhereLookupMultiNotNull,
				WhereLookupMultiNotContains,
				WhereLookupMultiContains
			};
		}

		IEnumerable<Func<IQueryable<TeamModel>, object>> ITestQueryProvider<TeamModel>.GetQueries()
		{
			return new Func<IQueryable<TeamModel>, object>[]
			{
				WhereUserNotNull,
				WhereUserNotEqual,
				WhereUserEqual,

				WhereUserMultiNotNull,
				WhereUserMultiNotContains,
				WhereUserMultiContains
			};
		}
	}
}
