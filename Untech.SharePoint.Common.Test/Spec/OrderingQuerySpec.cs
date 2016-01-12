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
	public class OrderingQuerySpec : IQueryTestsProvider<ProjectModel>
	{
		public IEnumerable<ProjectModel> OrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology);
		}

		public IEnumerable<ProjectModel> WhereOrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Status == "Approved")
				.OrderBy(n => n.Technology);
		}

		public IEnumerable<ProjectModel> OrderByWhereQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.Where(n => n.Status == "Approved");
		}

		public IEnumerable<ProjectModel> Take10OrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Take(10)
				.OrderBy(n => n.Technology);
		}

		public IEnumerable<ProjectModel> OrderByTake10Query(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.Take(10);
		}

		public IEnumerable<Tuple<string, string>> SelectOrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Select(n => new Tuple<string, string>(n.Title, n.Technology))
				.OrderBy(n => n.Item2);
		}

		public IEnumerable<ProjectModel> OrderByDescQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderByDescending(n => n.Technology);
		}

		public IEnumerable<ProjectModel> ThenByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenBy(n => n.Title);
		}

		public IEnumerable<ProjectModel> ThenByDescQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenByDescending(n => n.Title);
		}

		public IEnumerable<ProjectModel> ReverseQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenBy(n => n.Title)
				.Reverse();
		}

		public IEnumerable<QueryTest<ProjectModel>> GetQueryTests()
		{
			return new[]
			{
				QueryTest<ProjectModel>.Functional(OrderByQuery, EntityComparer.Default),

				QueryTest<ProjectModel>.Functional(WhereOrderByQuery, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(OrderByWhereQuery, EntityComparer.Default),

				QueryTest<ProjectModel>.Functional(SelectOrderByQuery).Throws<NotSupportedException>(),

				QueryTest<ProjectModel>.Functional(Take10OrderByQuery).Throws<NotSupportedException>(),
				QueryTest<ProjectModel>.Functional(OrderByTake10Query, EntityComparer.Default),

				QueryTest<ProjectModel>.Functional(OrderByDescQuery, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(ThenByQuery, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(ThenByDescQuery, EntityComparer.Default),
				QueryTest<ProjectModel>.Functional(ReverseQuery, EntityComparer.Default),
			};
		}
	}
}
