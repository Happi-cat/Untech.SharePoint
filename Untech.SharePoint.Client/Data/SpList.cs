using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Untech.SharePoint.Client.Data
{
	public sealed class SpList<T> : ISpList<T>, IQueryProvider
	{
		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Type ElementType
		{
			get { throw new NotImplementedException(); }
		}

		public Expression Expression
		{
			get { throw new NotImplementedException(); }
		}

		public IQueryProvider Provider
		{
			get { throw new NotImplementedException(); }
		}

		public void Add(T item)
		{
			throw new NotImplementedException();
		}

		public void Update(T item)
		{
			throw new NotImplementedException();
		}

		public IDataContext DataContext
		{
			get { throw new NotImplementedException(); }
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			throw new NotImplementedException();
		}

		public IQueryable CreateQuery(Expression expression)
		{
			throw new NotImplementedException();
		}

		public TResult Execute<TResult>(Expression expression)
		{
			throw new NotImplementedException();
		}

		public object Execute(Expression expression)
		{
			throw new NotImplementedException();
		}
	}
}