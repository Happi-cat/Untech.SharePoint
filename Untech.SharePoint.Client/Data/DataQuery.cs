using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class DataQuery<T> : IOrderedQueryable<T>, IQueryProvider
	{
		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Expression Expression { get; private set; }
		public Type ElementType { get; private set; }
		public IQueryProvider Provider { get; private set; }
		public IQueryable CreateQuery(Expression expression)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			throw new NotImplementedException();
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