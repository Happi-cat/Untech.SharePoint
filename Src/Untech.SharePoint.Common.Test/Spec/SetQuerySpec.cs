using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The set methods are All, Any, Concat, Contains, DefaultIfEmpty, Distinct, EqualAll, Except, Intersect, and Union.
	/// </summary>
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToAny")]
	public class SetQuerySpec : ITestQueryProvider<NewsModel>
	{
		#region [All]

		public object AllQuery(IQueryable<NewsModel> source)
		{
			return source.All(n => n.Created > DateTime.Now.AddMonths(-1));
		}

		public object WhereAllQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.All(n => n.Created > DateTime.Now.AddMonths(-1));
		}

		[NotSupportedQuery]
		public object SelectAllQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.All(n => n.Contains("DESCRIPTION"));
		}

		[NotSupportedQuery]
		public object Take10AllQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.All(n => n.Created > DateTime.Now.AddMonths(-1));
		}

		#endregion


		#region [Any]

		public object AnyQuery(IQueryable<NewsModel> source)
		{
			return source.Any();
		}

		public object WhereAnyQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Any();
		}

		public object SelectAnyQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Any();
		}

		public object Take10AnyQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Any();
		}

		public object AnyPQuery(IQueryable<NewsModel> source)
		{
			return source.Any(n => n.Description.StartsWith("STATIC"));
		}

		public object WhereAnyPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Any(n => n.Description.Contains("1") || n.Description.Contains("2"));
		}

		[NotSupportedQuery]
		public object SelectAnyPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Any(n => n.Contains("1") || n.Contains("2"));
		}

		[NotSupportedQuery]
		public object Take10AnyPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Any(n => n.Description.StartsWith("STATIC"));
		}

		#endregion


		public IEnumerable<Func<IQueryable<NewsModel>, object>> GetQueries()
		{
			return new Func<IQueryable<NewsModel>, object>[]
			{
				AllQuery,
				WhereAllQuery,
				SelectAllQuery,
				Take10AllQuery,

				AnyQuery,
				WhereAnyQuery,
				SelectAnyQuery,
				Take10AnyQuery,

				AnyPQuery,
				WhereAnyPQuery,
				SelectAnyPQuery,
				Take10AnyPQuery,
			};
		}
	}
}
