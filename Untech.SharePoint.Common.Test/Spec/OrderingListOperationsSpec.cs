using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The ordering methods are OrderBy, OrderByDescending, ThenBy, ThenByDescending, and Reverse.
	/// </summary>
	public class OrderingListOperationsSpec : ITestQueryProvider<ProjectModel>
	{
		public IEnumerable<ProjectModel> OrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ToList();
		}

		public IEnumerable<ProjectModel> WhereOrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Where(n => n.Status == "Approved")
				.OrderBy(n => n.Technology)
				.ToList();
		}

		public IEnumerable<ProjectModel> OrderByWhereQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.Where(n => n.Status == "Approved")
				.ToList();
		}

		public IEnumerable<ProjectModel> Take10OrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Take(10)
				.OrderBy(n => n.Technology)
				.ToList();
		}

		public IEnumerable<ProjectModel> OrderByTake10Query(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.Take(10)
				.ToList();
		}

		public IEnumerable<Tuple<string, string>> SelectOrderByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.Select(n => new Tuple<string, string>(n.Title, n.Technology))
				.OrderBy(n => n.Item2)
				.ToList();
		}

		public IEnumerable<ProjectModel> OrderByDescQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderByDescending(n => n.Technology)
				.ToList();
		}

		public IEnumerable<ProjectModel> ThenByQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenBy(n => n.Title)
				.ToList();
		}

		public IEnumerable<ProjectModel> ThenByDescQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenByDescending(n => n.Title)
				.ToList();
		}

		public IEnumerable<ProjectModel> ReverseQuery(IQueryable<ProjectModel> source)
		{
			return source
				.OrderBy(n => n.Technology)
				.ThenBy(n => n.Title)
				.Reverse()
				.ToList();
		}

		public IEnumerable<TestQuery<ProjectModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<ProjectModel>.Create(OrderByQuery, EntitySequenceComparer.Default),

				TestQuery<ProjectModel>.Create(WhereOrderByQuery, EntitySequenceComparer.Default),
				TestQuery<ProjectModel>.Create(OrderByWhereQuery, EntitySequenceComparer.Default),

				TestQuery<ProjectModel>.Create(SelectOrderByQuery).Throws<NotSupportedException>(),

				TestQuery<ProjectModel>.Create(Take10OrderByQuery).Throws<NotSupportedException>(),
				TestQuery<ProjectModel>.Create(OrderByTake10Query, EntitySequenceComparer.Default),

				TestQuery<ProjectModel>.Create(OrderByDescQuery, EntitySequenceComparer.Default),
				TestQuery<ProjectModel>.Create(ThenByQuery, EntitySequenceComparer.Default),
				TestQuery<ProjectModel>.Create(ThenByDescQuery, EntitySequenceComparer.Default),
				TestQuery<ProjectModel>.Create(ReverseQuery, EntitySequenceComparer.Default),
			};
		}
	}
}
