using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The set methods are All, Any, Concat, Contains, DefaultIfEmpty, Distinct, EqualAll, Except, Intersect, and Union.
	/// </summary>
	public class SetListOperationsSpec : ITestQueryProvider<NewsModel>
	{
		#region [All]

		public bool AllQuery(IQueryable<NewsModel> source)
		{
			return source.All(n => n.Created > DateTime.Now.AddMonths(-1));
		}

		public bool WhereAllQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.All(n => n.Created > DateTime.Now.AddMonths(-1));
		}

		public bool SelectAllQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.All(n => n.Contains("DESCRIPTION"));
		}

		public bool Take10AllQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.All(n => n.Created > DateTime.Now.AddMonths(-1));
		}

		#endregion


		#region [Any]

		public bool AnyQuery(IQueryable<NewsModel> source)
		{
			return source.Any();
		}

		public bool WhereAnyQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Any();
		}

		public bool SelectAnyQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Any();
		}

		public bool Take10AnyQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Any();
		}

		public bool AnyPQuery(IQueryable<NewsModel> source)
		{
			return source.Any(n => n.Description.StartsWith("STATIC"));
		}

		public bool WhereAnyPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Any(n => n.Description.Contains("1") || n.Description.Contains("2"));
		}

		public bool SelectAnyPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Any(n => n.Contains("1") || n.Contains("2"));
		}

		public bool Take10AnyPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Any(n => n.Description.StartsWith("STATIC"));
		}

		#endregion


		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<NewsModel>.Create(AllQuery),
				TestQuery<NewsModel>.Create(WhereAllQuery),
				TestQuery<NewsModel>.Create(SelectAllQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(Take10AllQuery).Throws<NotSupportedException>(),

				TestQuery<NewsModel>.Create(AnyQuery),
				TestQuery<NewsModel>.Create(WhereAnyQuery),
				TestQuery<NewsModel>.Create(SelectAnyQuery),
				TestQuery<NewsModel>.Create(Take10AnyQuery),

				TestQuery<NewsModel>.Create(AnyPQuery),
				TestQuery<NewsModel>.Create(WhereAnyPQuery),
				TestQuery<NewsModel>.Create(SelectAnyPQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(Take10AnyPQuery).Throws<NotSupportedException>(),
			};
		}
	}
}
