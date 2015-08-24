using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Untech.SharePoint.Client.Data
{
	public sealed class SpList<T> : IQueryable<T>, ISpList<T>
	{
		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
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
	}
}