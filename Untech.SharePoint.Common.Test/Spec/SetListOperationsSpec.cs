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
		public bool AllQuery(IQueryable<NewsModel> source)
		{
			return source.All(n => n.Created > DateTime.Now.AddMonths(-1));
		}

		public bool AnyQuery(IQueryable<NewsModel> source)
		{
			return source.Any();
		}

		public bool AnyPQuery(IQueryable<NewsModel> source)
		{
			return source.Any(n => n.Description.StartsWith("STATIC"));
		}

		public IEnumerable<TestQuery<NewsModel>> GetTestQueries()
		{
			return new[]
			{
				TestQuery<NewsModel>.Create(AllQuery),
				TestQuery<NewsModel>.Create(AnyQuery),
				TestQuery<NewsModel>.Create(AnyPQuery),
			};
		}
	}
}
