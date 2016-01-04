using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Spec
{
	/// <summary>
	/// The aggregate methods are Aggregate, Average, Count, LongCount, Max, Min, and Sum.
	/// </summary>
	public class AggregateListOperationsSpec : ITestQueryProvider<NewsModel>
	{
		public int CountQuery(IQueryable<Entity> source)
		{
			return source.Count();
		}

		public int CountPQuery(IQueryable<NewsModel> source)
		{
			return source.Count(n => n.Description.StartsWith("ABC"));
		}

		public int MinPQuery(IQueryable<Entity> source)
		{
			return source.Min(n => n.Id);
		}

		public int MaxPQuery(IQueryable<Entity> source)
		{
			return source.Max(n => n.Id);
		}

		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new []
			{
				TestQuery<NewsModel>.Create(CountQuery),
				TestQuery<NewsModel>.Create(CountPQuery),
				TestQuery<NewsModel>.Create(MinPQuery),
				TestQuery<NewsModel>.Create(MaxPQuery)
			};
		}
	}
}
