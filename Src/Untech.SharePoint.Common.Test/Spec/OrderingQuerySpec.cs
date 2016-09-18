using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.Comparers;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The ordering methods are OrderBy, OrderByDescending, ThenBy, ThenByDescending, and Reverse.
	/// </summary>
	public class OrderingQuerySpec : ITestQueryProvider<ProjectModel>
	{
		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<ProjectModel> OrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology);
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<ProjectModel> WhereOrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Status == "Approved")
				.OrderBy(n => n.Technology);
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<ProjectModel> OrderByWhereQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.Where(n => n.Status == "Approved");
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public IEnumerable<ProjectModel> Take10OrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Take(10)
				.OrderBy(n => n.Technology);
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<ProjectModel> OrderByTake10Query(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.Take(10);
		}

		[NotSupportedQuery]
		public IEnumerable<Tuple<string, string>> SelectOrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Select(n => new Tuple<string, string>(n.Title, n.Technology))
				.OrderBy(n => n.Item2);
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<ProjectModel> OrderByDescQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderByDescending(n => n.Technology);
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<ProjectModel> ThenByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenBy(n => n.Title);
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<ProjectModel> ThenByDescQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenByDescending(n => n.Title);
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<ProjectModel> ReverseQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenBy(n => n.Title)
				.Reverse();
		}

		public IEnumerable<Func<IQueryable<ProjectModel>, object>> GetQueries()
		{
			return new Func<IQueryable<ProjectModel>, object>[]
			{
				OrderByQuery,

				WhereOrderByQuery,
				OrderByWhereQuery,

				SelectOrderByQuery,

				Take10OrderByQuery,
				OrderByTake10Query,

				OrderByDescQuery,
				ThenByQuery,
				ThenByDescQuery,
				ReverseQuery,
			};
		}
	}
}
