using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The aggregate methods are Aggregate, Average, Count, LongCount, Max, Min, and Sum.
	/// </summary>
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToCount")]
	public class AggregateQuerySpec : IQueryTestsProvider<NewsModel>
	{
		#region [Count]

		public int CountQuery(IQueryable<Entity> source)
		{
			return source.Count();
		}

		public int CountPQuery(IQueryable<NewsModel> source)
		{
			return source.Count(n => n.Description.StartsWith("DESCRIPTION"));
		}

		public int WhereCountQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.Contains("DESCRIPTION"))
				.Count();
		}

		public int WhereCountPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.Contains("DESCRIPTION"))
				.Count(n => n.Title.Contains("lorem"));
		}

		public int SelectCountQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Count();
		}

		public int Take10CountQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Count();
		}

		#endregion


		#region [Min]

		public int MinPQuery(IQueryable<Entity> source)
		{
			return source.Min(n => n.Id);
		}

		public int WhereMinPQuery(IQueryable<Entity> source)
		{
			return source
				.Where(n => n.Title.Contains("lorem"))
				.Min(n => n.Id);
		}

		public int SelectMinQuery(IQueryable<Entity> source)
		{
			return source
				.Select(n => n.Id)
				.Min();
		}

		public int SelectMinPQuery(IQueryable<Entity> source)
		{
			return source
				.Select(n => n.Id)
				.Min(n => n);
		}

		public int Take10MinPQuery(IQueryable<Entity> source)
		{
			return source
				.Take(10)
				.Min(n => n.Id);
		}

		#endregion


		#region [Max]

		public int MaxPQuery(IQueryable<Entity> source)
		{
			return source.Max(n => n.Id);
		}

		public int WhereMaxPQuery(IQueryable<Entity> source)
		{
			return source
				.Where(n => n.Title.Contains("lorem"))
				.Max(n => n.Id);
		}

		public int SelectMaxQuery(IQueryable<Entity> source)
		{
			return source
				.Select(n => n.Id)
				.Max();
		}

		public int SelectMaxPQuery(IQueryable<Entity> source)
		{
			return source
				.Select(n => n.Id)
				.Max(n => n);
		}

		public int Take10MaxPQuery(IQueryable<Entity> source)
		{
			return source
				.Take(10)
				.Max(n => n.Id);
		}

		#endregion


		public IEnumerable<QueryTest<NewsModel>> GetQueryTests()
		{
			return new[]
			{
				QueryTest<NewsModel>.Functional(CountQuery),
				QueryTest<NewsModel>.Functional(CountPQuery),
				QueryTest<NewsModel>.Functional(WhereCountQuery),
				QueryTest<NewsModel>.Functional(WhereCountPQuery),
				QueryTest<NewsModel>.Functional(SelectCountQuery),
				QueryTest<NewsModel>.Functional(Take10CountQuery), 

				QueryTest<NewsModel>.Functional(MinPQuery),
				QueryTest<NewsModel>.Functional(WhereMinPQuery),
				QueryTest<NewsModel>.Functional(SelectMinQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(SelectMinPQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(Take10MinPQuery), 

				QueryTest<NewsModel>.Functional(MaxPQuery),
				QueryTest<NewsModel>.Functional(WhereMaxPQuery),
				QueryTest<NewsModel>.Functional(SelectMaxQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(SelectMaxPQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(Take10MaxPQuery), 
			};
		}
	}
}
