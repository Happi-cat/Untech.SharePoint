using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// Paging operations return a single, specific element from a sequence. The element methods are ElementAt, First, FirstOrDefault, Last, LastOrDefault, Single, Skip, Take, TakeWhile.
	/// </summary>
	public class PagingListOperationsSpec : ITestQueryProvider<NewsModel>
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


		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<NewsModel>.Create(SingleQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(SelectSingleQuery),
				TestQuery<NewsModel>.Create(Take10SingleQuery),
				TestQuery<NewsModel>.Create(SingleOrDefaultQuery, EntityComparer.Default),

				TestQuery<NewsModel>.Create(SinglePQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(SelectSinglePQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(Take10SinglePQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(SingleOrDefaultPQuery, EntityComparer.Default),

				TestQuery<NewsModel>.Create(ElementAtQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(Take10ElementAtQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(SelectElementAtQuery),
				TestQuery<NewsModel>.Create(ElementAtOrDefaultQuery, EntityComparer.Default),
				
				TestQuery<NewsModel>.Create(FirstQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(Take10FirstQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(SelectFirstQuery),
				TestQuery<NewsModel>.Create(WhereFirstQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(OrderByFirstQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(FirstOrDefaultQuery, EntityComparer.Default),

				TestQuery<NewsModel>.Create(FirstPQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(Take10FirstPQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(SelectFirstPQuery),
				TestQuery<NewsModel>.Create(WhereFirstPQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(FirstPOrDefaultQuery, EntityComparer.Default),
				
				TestQuery<NewsModel>.Create(LastQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(Take10LastQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(SelectLastQuery),
				TestQuery<NewsModel>.Create(WhereLastQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(OrderByLastQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(LastOrDefaultQuery, EntityComparer.Default),

				TestQuery<NewsModel>.Create(LastPQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(Take10LastPQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(SelectLastPQuery),
				TestQuery<NewsModel>.Create(WhereLastPQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(LastPOrDefaultQuery, EntityComparer.Default),

				TestQuery<NewsModel>.Create(TakeQuery, EntitySequenceComparer.Default),
				TestQuery<NewsModel>.Create(SelectTakeQuery, SequenceComparer<string>.Default),
				TestQuery<NewsModel>.Create(TakeSelectQuery, SequenceComparer<string>.Default),
				TestQuery<NewsModel>.Create(WhereTakeQuery, EntitySequenceComparer.Default),
				TestQuery<NewsModel>.Create(TakeWhereQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(OrderByTakeQuery, EntitySequenceComparer.Default),
				TestQuery<NewsModel>.Create(TakeOrderByQuery).Throws<NotSupportedException>(),
				TestQuery<NewsModel>.Create(TakeTakeQuery).Throws<NotSupportedException>(),
			};
		}
	}
}
