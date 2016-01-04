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
			return source.OrderBy(n => n.Technology).ToList();
		}

		public IEnumerable<ProjectModel> OrderByDescQuery(IQueryable<ProjectModel> source)
		{
			return source.OrderByDescending(n => n.Technology).ToList();
		}

		public IEnumerable<ProjectModel> ThenByQuery(IQueryable<ProjectModel> source)
		{
			return source.OrderBy(n => n.Technology).ThenBy(n => n.Title).ToList();
		}

		public IEnumerable<ProjectModel> ThenByDescQuery(IQueryable<ProjectModel> source)
		{
			return source.OrderBy(n => n.Technology).ThenByDescending(n => n.Title).ToList();
		}

		public IEnumerable<ProjectModel> ReverseQuery(IQueryable<ProjectModel> source)
		{
			return source.OrderBy(n => n.Technology).ThenBy(n => n.Title).Reverse().ToList();
		}

		public IEnumerable<TestQuery<ProjectModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<ProjectModel>.Create(OrderByQuery, EntitySequenceComparer.Default),
				TestQuery<ProjectModel>.Create(OrderByDescQuery, EntitySequenceComparer.Default),
				TestQuery<ProjectModel>.Create(ThenByQuery, EntitySequenceComparer.Default),
				TestQuery<ProjectModel>.Create(ThenByDescQuery, EntitySequenceComparer.Default),
				TestQuery<ProjectModel>.Create(ReverseQuery, EntitySequenceComparer.Default),
			};
		}
	}
}
