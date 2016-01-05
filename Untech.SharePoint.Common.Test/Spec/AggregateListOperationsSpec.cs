using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The aggregate methods are Aggregate, Average, Count, LongCount, Max, Min, and Sum.
	/// </summary>
	public class AggregateListOperationsSpec : ITestQueryProvider<NewsModel>
	{
		#region [Count]

		public int CountQuery(IQueryable<Entity> source)
		{
			return source.Count();
		}

		public int CountPQuery(IQueryable<NewsModel> source)
		{
			return source.Count(n => n.Description.StartsWith("ABC"));
		}

		public int WhereCountQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.Contains("lorem"))
				.Count();
		}

		public int WhereCountPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.Contains("lorem"))
				.Count(n => n.Title.StartsWith("lorem"));
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


		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<NewsModel>.Create(CountQuery),
				TestQuery<NewsModel>.Create(CountPQuery),
				TestQuery<NewsModel>.Create(WhereCountQuery),
				TestQuery<NewsModel>.Create(WhereCountPQuery),
				TestQuery<NewsModel>.Create(SelectCountQuery),
				TestQuery<NewsModel>.Create(Take10CountQuery), 

				TestQuery<NewsModel>.Create(MinPQuery),
				TestQuery<NewsModel>.Create(WhereMinPQuery),
				TestQuery<NewsModel>.Create(SelectMinQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(SelectMinPQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(Take10MinPQuery), 

				TestQuery<NewsModel>.Create(MaxPQuery),
				TestQuery<NewsModel>.Create(WhereMaxPQuery),
				TestQuery<NewsModel>.Create(SelectMaxQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(SelectMaxPQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(Take10MaxPQuery), 
			};
		}
	}
}
