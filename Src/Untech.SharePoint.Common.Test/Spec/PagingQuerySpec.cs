using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Untech.SharePoint.Spec.Models;
using Untech.SharePoint.TestTools.Comparers;
using Untech.SharePoint.TestTools.QueryTests;

namespace Untech.SharePoint.Spec
{
	/// <summary>
	/// Paging operations return a single, specific element from a sequence. The element methods are ElementAt, First, FirstOrDefault, Last, LastOrDefault, Single, Skip, Take, TakeWhile.
	/// </summary>
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToSingle")]
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToSingleOrDefault")]
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToFirst")]
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToLast")]
	public class PagingQuerySpec : ITestQueryProvider<NewsModel>
	{
		#region [Single]

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel SingleQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description == "SINGLETON")
				.Single();
		}

		public string SelectSingleQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description == "SINGLETON")
				.Select(n => n.Description)
				.Single();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel Take10SingleQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description == "SINGLETON")
				.Take(10)
				.Single();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel SinglePQuery(IQueryable<NewsModel> source)
		{
			return source
				.Single(n => n.Description == "SINGLETON");
		}

		[NotSupportedQuery]
		public string SelectSinglePQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Single(n => n == "SINGLETON");
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public NewsModel Take10SinglePQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Single(n => n.Description == "SINGLETON");
		}

		[QueryComparer(typeof(EntityComparer))]
		[EmptyResultQuery]
		public NewsModel SingleOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description == "UNKOWN DESCRIPTION")
				.SingleOrDefault();
		}

		[QueryComparer(typeof(EntityComparer))]
		[EmptyResultQuery]
		public NewsModel SingleOrDefaultPQuery(IQueryable<NewsModel> source)
		{
			return source
				.SingleOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		#endregion


		#region [ElementAt]

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel ElementAtQuery(IQueryable<NewsModel> source)
		{
			return source.ElementAt(2);
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public NewsModel Take10ElementAtQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.ElementAt(2);
		}

		public string SelectElementAtQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Title)
				.ElementAt(2);
		}

		[QueryComparer(typeof(EntityComparer))]
		[EmptyResultQuery]
		public NewsModel ElementAtOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.ElementAtOrDefault(1000);
		}

		#endregion


		#region [First]

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel FirstQuery(IQueryable<NewsModel> source)
		{
			return source.First();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel Take10FirstQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.First();
		}

		public string SelectFirstQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Title)
				.First();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel WhereFirstQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.First();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel OrderByFirstQuery(IQueryable<NewsModel> source)
		{
			return source
				.OrderBy(n => n.Title)
				.First();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel FirstOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.FirstOrDefault();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel FirstPQuery(IQueryable<NewsModel> source)
		{
			return source.First(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public NewsModel Take10FirstPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.First(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel WhereFirstPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.First(n => n.Description.Contains("1") || n.Description.Contains("2"));
		}

		[NotSupportedQuery]
		public string SelectFirstPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.First(n => n == "DESCRIPTION 1" || n == "DESCRIPTION 2");
		}

		[QueryComparer(typeof(EntityComparer))]
		[EmptyResultQuery]
		public NewsModel FirstPOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.FirstOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		#endregion


		#region [Last]

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel LastQuery(IQueryable<NewsModel> source)
		{
			return source.Last();
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public NewsModel Take10LastQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Last();
		}

		public string SelectLastQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Title)
				.Last();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel WhereLastQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Last();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel OrderByLastQuery(IQueryable<NewsModel> source)
		{
			return source
				.OrderBy(n => n.Title)
				.Last();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel LastOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.LastOrDefault();
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel LastPQuery(IQueryable<NewsModel> source)
		{
			return source.Last(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public NewsModel Take10LastPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Last(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		[QueryComparer(typeof(EntityComparer))]
		public NewsModel WhereLastPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Last(n => n.Description.Contains("1") || n.Description.Contains("2"));
		}

		[NotSupportedQuery]
		public string SelectLastPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Last(n => n == "DESCRIPTION 1" || n == "DESCRIPTION 2");
		}

		[QueryComparer(typeof(EntityComparer))]
		[EmptyResultQuery]
		public NewsModel LastPOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.LastOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		#endregion


		#region [Take]

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<NewsModel> TakeQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10);
		}

		public IEnumerable<string> SelectTakeQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Title)
				.Take(10);
		}

		public IEnumerable<string> TakeSelectQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Select(n => n.Title);
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<NewsModel> WhereTakeQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.Contains("DESCRIPTION"))
				.Take(10);
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public IEnumerable<NewsModel> TakeWhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Where(n => n.Description.Contains("DESCRIPTION"));
		}

		[QueryComparer(typeof(EntityComparer))]
		public IEnumerable<NewsModel> OrderByTakeQuery(IQueryable<NewsModel> source)
		{
			return source
				.OrderBy(n => n.Title)
				.Take(10);
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public IEnumerable<NewsModel> TakeOrderByQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.OrderBy(n => n.Title);
		}

		[QueryComparer(typeof(EntityComparer))]
		[NotSupportedQuery]
		public IEnumerable<NewsModel> TakeTakeQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Take(2);
		}

		#endregion


		public IEnumerable<Func<IQueryable<NewsModel>, object>> GetQueries()
		{
			return new Func<IQueryable<NewsModel>, object>[]
			{
				SingleQuery,
				SelectSingleQuery,
				Take10SingleQuery,
				SingleOrDefaultQuery,

				SinglePQuery,
				SelectSinglePQuery,
				Take10SinglePQuery,
				SingleOrDefaultPQuery,

				ElementAtQuery,
				Take10ElementAtQuery,
				SelectElementAtQuery,
				ElementAtOrDefaultQuery,

				FirstQuery,
				Take10FirstQuery,
				SelectFirstQuery,
				WhereFirstQuery,
				OrderByFirstQuery,
				FirstOrDefaultQuery,

				FirstPQuery,
				Take10FirstPQuery,
				SelectFirstPQuery,
				WhereFirstPQuery,
				FirstPOrDefaultQuery,

				LastQuery,
				Take10LastQuery,
				SelectLastQuery,
				WhereLastQuery,
				OrderByLastQuery,
				LastOrDefaultQuery,

				LastPQuery,
				Take10LastPQuery,
				SelectLastPQuery,
				WhereLastPQuery,
				LastPOrDefaultQuery,

				TakeQuery,
				SelectTakeQuery,
				TakeSelectQuery,
				WhereTakeQuery,
				TakeWhereQuery,
				OrderByTakeQuery,
				TakeOrderByQuery,
				TakeTakeQuery,
			};
		}
	}
}
