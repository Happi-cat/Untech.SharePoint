using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data
{
	internal class SpList<T> : ISpList<T>
	{
		public SpList(ISpItemsProvider itemsProvider)
		{
			Guard.CheckNotNull("itemsProvider", itemsProvider);

			ItemsProvider = itemsProvider;
			Expression = SpQueryable.MakeFakeGetAll(typeof(T), itemsProvider);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return SpLinqQueryProvider
				.RewriteAndCompile<IEnumerable<T>>(Expression)()
				.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Expression Expression { get; private set; }
		public Type ElementType { get { return typeof(T); } }

		public ISpItemsProvider ItemsProvider { get; private set; }

		public IQueryProvider Provider
		{
			get { return SpLinqQueryProvider.Instance; }
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