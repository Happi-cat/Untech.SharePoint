using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Spec.Models;
using Untech.SharePoint.Common.TestTools.QueryTests;

namespace Untech.SharePoint.Common.Spec
{
	/// <summary>
	/// The aggregate methods are Aggregate, Average, Count, LongCount, Max, Min, and Sum.
	/// </summary>
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToCount")]
	public class AggregateQuerySpec : ITestQueryProvider<NewsModel>
	{
		#region [Count]

		public object CountQuery(IQueryable<Entity> source)
		{
			return source.Count();
		}

		public object CountPQuery(IQueryable<NewsModel> source)
		{
			return source.Count(n => n.Description.StartsWith("DESCRIPTION"));
		}

		public object WhereCountQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.Contains("DESCRIPTION"))
				.Count();
		}

		public object WhereCountPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.Contains("DESCRIPTION"))
				.Count(n => n.Title.Contains("lorem"));
		}

		public object SelectCountQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Count();
		}

		public object Take10CountQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Count();
		}

		#endregion

		#region [Min]

		public object MinPQuery(IQueryable<Entity> source)
		{
			return source.Min(n => n.Id);
		}

		public object WhereMinPQuery(IQueryable<Entity> source)
		{
			return source
				.Where(n => n.Title.Contains("lorem"))
				.Min(n => n.Id);
		}

		public object SelectMinQuery(IQueryable<Entity> source)
		{
			return source
				.Select(n => n.Id)
				.Min();
		}

		[NotSupportedQuery]
		public object SelectMinPQuery(IQueryable<Entity> source)
		{
			return source
				.Select(n => n.Id)
				.Min(n => n);
		}

		public object Take10MinPQuery(IQueryable<Entity> source)
		{
			return source
				.Take(10)
				.Min(n => n.Id);
		}

		#endregion

		#region [Max]

		public object MaxPQuery(IQueryable<Entity> source)
		{
			return source.Max(n => n.Id);
		}

		public object WhereMaxPQuery(IQueryable<Entity> source)
		{
			return source
				.Where(n => n.Title.Contains("lorem"))
				.Max(n => n.Id);
		}

		public object SelectMaxQuery(IQueryable<Entity> source)
		{
			return source
				.Select(n => n.Id)
				.Max();
		}

		[NotSupportedQuery]
		public object SelectMaxPQuery(IQueryable<Entity> source)
		{
			return source
				.Select(n => n.Id)
				.Max(n => n);
		}

		public object Take10MaxPQuery(IQueryable<Entity> source)
		{
			return source
				.Take(10)
				.Max(n => n.Id);
		}

		#endregion

		public IEnumerable<Func<IQueryable<NewsModel>, object>> GetQueries()
		{
			return new Func<IQueryable<NewsModel>, object>[]
			{
				CountQuery,
				CountPQuery,
				WhereCountQuery,
				WhereCountPQuery,
				SelectCountQuery,
				Take10CountQuery,

				MinPQuery,
				WhereMinPQuery,
				SelectMinQuery,
				SelectMinPQuery,
				Take10MinPQuery,

				MaxPQuery,
				WhereMaxPQuery,
				SelectMaxQuery,
				SelectMaxPQuery,
				Take10MaxPQuery
			};
		}
	}
}
