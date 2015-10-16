using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	public class FakeQueryable<T> : IOrderedQueryable<T>, IQueryProvider
	{
		public FakeQueryable()
		{
			Expression = SpQueryable.MakeFakeFetch(typeof(T), null);
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

		public Action<Expression>  ExpressionExecutor { get; set; }

		public IQueryProvider Provider
		{
			get { return this; }
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return new FakeQueryable<T>(expression)
			{
				ExpressionExecutor = ExpressionExecutor
			};
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new FakeQueryable<TElement>(expression)
			{
				ExpressionExecutor = ExpressionExecutor
			};
		}

		public object Execute(Expression expression)
		{
			if (ExpressionExecutor != null)
			{
				ExpressionExecutor(expression);
			}
			return null;
		}

		public TResult Execute<TResult>(Expression expression)
		{
			if (ExpressionExecutor != null)
			{
				ExpressionExecutor(expression);
			}
			return default(TResult);
		}
	}
}