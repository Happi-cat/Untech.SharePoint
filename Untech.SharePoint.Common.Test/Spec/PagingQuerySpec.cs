using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.Comparers;
using Untech.SharePoint.Common.Test.Tools.QueryTests;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// Paging operations return a single, specific element from a sequence. The element methods are ElementAt, First, FirstOrDefault, Last, LastOrDefault, Single, Skip, Take, TakeWhile.
	/// </summary>
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToSingle")]
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToSingleOrDefault")]
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToFirst")]
	[SuppressMessage("ReSharper", "ReplaceWithSingleCallToLast")]
	public class PagingQuerySpec : IQueryTestsProvider<NewsModel>
	{
		#region [Single]

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

		public NewsModel Take10SingleQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description == "SINGLETON")
				.Take(10)
				.Single();
		}

		public NewsModel SinglePQuery(IQueryable<NewsModel> source)
		{
			return source
				.Single(n => n.Description == "SINGLETON");
		}

		public string SelectSinglePQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Single(n => n == "SINGLETON");
		}

		public NewsModel Take10SinglePQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Single(n => n.Description == "SINGLETON");
		}

		public NewsModel SingleOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description == "UNKOWN DESCRIPTION")
				.SingleOrDefault();
		}

		public NewsModel SingleOrDefaultPQuery(IQueryable<NewsModel> source)
		{
			return source
				.SingleOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		#endregion


		#region [ElementAt]

		public NewsModel ElementAtQuery(IQueryable<NewsModel> source)
		{
			return source.ElementAt(2);
		}

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

		public NewsModel ElementAtOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.ElementAtOrDefault(1000);
		}

		#endregion


		#region [First]

		public NewsModel FirstQuery(IQueryable<NewsModel> source)
		{
			return source.First();
		}

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

		public NewsModel WhereFirstQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.First();
		}

		public NewsModel OrderByFirstQuery(IQueryable<NewsModel> source)
		{
			return source
				.OrderBy(n => n.Title)
				.First();
		}

		public NewsModel FirstOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.FirstOrDefault();
		}

		public NewsModel FirstPQuery(IQueryable<NewsModel> source)
		{
			return source.First(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		public NewsModel Take10FirstPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.First(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		public NewsModel WhereFirstPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n=> n.Description.StartsWith("DESCRIPTION"))
				.First(n => n.Description.Contains("1") || n.Description.Contains("2"));
		}

		public string SelectFirstPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.First(n => n == "DESCRIPTION 1" || n == "DESCRIPTION 2");
		}

		public NewsModel FirstPOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.FirstOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		#endregion


		#region [Last]

		public NewsModel LastQuery(IQueryable<NewsModel> source)
		{
			return source.Last();
		}

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

		public NewsModel WhereLastQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Last();
		}

		public NewsModel OrderByLastQuery(IQueryable<NewsModel> source)
		{
			return source
				.OrderBy(n => n.Title)
				.Last();
		}

		public NewsModel LastOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.LastOrDefault();
		}

		public NewsModel LastPQuery(IQueryable<NewsModel> source)
		{
			return source.Last(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		public NewsModel Take10LastPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Last(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		public NewsModel WhereLastPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.StartsWith("DESCRIPTION"))
				.Last(n => n.Description.Contains("1") || n.Description.Contains("2"));
		}

		public string SelectLastPQuery(IQueryable<NewsModel> source)
		{
			return source
				.Select(n => n.Description)
				.Last(n => n == "DESCRIPTION 1" || n == "DESCRIPTION 2");
		}

		public NewsModel LastPOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.LastOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		#endregion


		#region [Take]

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

		public IEnumerable<NewsModel> WhereTakeQuery(IQueryable<NewsModel> source)
		{
			return source
				.Where(n => n.Description.Contains("DESCRIPTION"))
				.Take(10);
		}

		public IEnumerable<NewsModel> TakeWhereQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Where(n => n.Description.Contains("DESCRIPTION"));
		}

		public IEnumerable<NewsModel> OrderByTakeQuery(IQueryable<NewsModel> source)
		{
			return source
				.OrderBy(n => n.Title)
				.Take(10);
		}

		public IEnumerable<NewsModel> TakeOrderByQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.OrderBy(n => n.Title);
		}

		public IEnumerable<NewsModel> TakeTakeQuery(IQueryable<NewsModel> source)
		{
			return source
				.Take(10)
				.Take(2);
		}

		#endregion


		public IEnumerable<QueryTest<NewsModel>> GetQueryTests()
		{
			return new[]
			{
				QueryTest<NewsModel>.Functional(SingleQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(SelectSingleQuery),
				QueryTest<NewsModel>.Functional(Take10SingleQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(SingleOrDefaultQuery, EntityComparer.Default),

				QueryTest<NewsModel>.Functional(SinglePQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(SelectSinglePQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(Take10SinglePQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(SingleOrDefaultPQuery, EntityComparer.Default),

				QueryTest<NewsModel>.Functional(ElementAtQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(Take10ElementAtQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(SelectElementAtQuery),
				QueryTest<NewsModel>.Functional(ElementAtOrDefaultQuery, EntityComparer.Default),
				
				QueryTest<NewsModel>.Functional(FirstQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(Take10FirstQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(SelectFirstQuery),
				QueryTest<NewsModel>.Functional(WhereFirstQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(OrderByFirstQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(FirstOrDefaultQuery, EntityComparer.Default),

				QueryTest<NewsModel>.Functional(FirstPQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(Take10FirstPQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(SelectFirstPQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(WhereFirstPQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(FirstPOrDefaultQuery, EntityComparer.Default),
				
				QueryTest<NewsModel>.Functional(LastQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(Take10LastQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(SelectLastQuery),
				QueryTest<NewsModel>.Functional(WhereLastQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(OrderByLastQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(LastOrDefaultQuery, EntityComparer.Default),

				QueryTest<NewsModel>.Functional(LastPQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(Take10LastPQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(SelectLastPQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(WhereLastPQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(LastPOrDefaultQuery, EntityComparer.Default),

				QueryTest<NewsModel>.Functional(TakeQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(SelectTakeQuery),
				QueryTest<NewsModel>.Functional(TakeSelectQuery),
				QueryTest<NewsModel>.Functional(WhereTakeQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(TakeWhereQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(OrderByTakeQuery, EntityComparer.Default),
				QueryTest<NewsModel>.Functional(TakeOrderByQuery).Throws<NotSupportedException>(),
				QueryTest<NewsModel>.Functional(TakeTakeQuery).Throws<NotSupportedException>(),
			};
		}
	}
}
