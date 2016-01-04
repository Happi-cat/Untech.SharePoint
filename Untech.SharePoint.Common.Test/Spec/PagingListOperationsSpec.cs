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
		public NewsModel SingleQuery(IQueryable<NewsModel> source)
		{
			return source.Where(n => n.Description == "SINGLETON").Single();
		}

		public NewsModel SinglePQuery(IQueryable<NewsModel> source)
		{
			return source.Single(n => n.Description == "SINGLETON");
		}

		public NewsModel SingleOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.Where(n => n.Description == "UNKOWN DESCRIPTION").SingleOrDefault();
		}

		public NewsModel SingleOrDefaultPQuery(IQueryable<NewsModel> source)
		{
			return source.SingleOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		public NewsModel ElementAtQuery(IQueryable<NewsModel> source)
		{
			return source.ElementAt(2);
		}

		public NewsModel ElementAtOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.ElementAtOrDefault(1000);
		}

		public NewsModel FirstQuery(IQueryable<NewsModel> source)
		{
			return source.First();
		}

		public NewsModel FirstOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.FirstOrDefault();
		}

		public NewsModel FirstPQuery(IQueryable<NewsModel> source)
		{
			return source.First(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		public NewsModel FirstPOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.FirstOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		public NewsModel LastQuery(IQueryable<NewsModel> source)
		{
			return source.Last();
		}

		public NewsModel LastOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.LastOrDefault();
		}

		public NewsModel LastPQuery(IQueryable<NewsModel> source)
		{
			return source.Last(n => n.Description == "DESCRIPTION 1" || n.Description == "DESCRIPTION 2");
		}

		public NewsModel LastPOrDefaultQuery(IQueryable<NewsModel> source)
		{
			return source.LastOrDefault(n => n.Description == "UNKOWN DESCRIPTION");
		}

		public IEnumerable<NewsModel> TakeQuery(IQueryable<NewsModel> source)
		{
			return source.Take(2).ToList();
		}


		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<NewsModel>.Create(SingleQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(SinglePQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(SingleOrDefaultQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(SingleOrDefaultPQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(ElementAtQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(ElementAtOrDefaultQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(FirstQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(FirstOrDefaultQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(FirstPQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(FirstPOrDefaultQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(LastQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(LastOrDefaultQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(LastPQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(LastPOrDefaultQuery, EntityComparer.Default),
				TestQuery<NewsModel>.Create(TakeQuery, EntitySequenceComparer.Default),
			};
		}
	}
}
