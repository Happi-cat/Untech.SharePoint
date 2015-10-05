using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	public class FakeQueryable<T> : IQueryable<T>, IQueryProvider
	{
		public FakeQueryable()
		{
			Expression = SpQueryable.MakeAsQueryable(typeof(T), SpQueryable.MakeGetSpListItems(typeof(T), null, new QueryModel()));
		}

		protected FakeQueryable(Expression node)
		{
			Expression = node;
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Expression Expression { get; private set; }

		public Type ElementType { get { return typeof(T); } }

		public IQueryProvider Provider
		{
			get { return this; }
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return new FakeQueryable<T>(expression);
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new FakeQueryable<TElement>(expression);
		}

		public object Execute(Expression expression)
		{
			throw new NotImplementedException();
		}

		public TResult Execute<TResult>(Expression expression)
		{
			throw new NotImplementedException();
		}
	}
}