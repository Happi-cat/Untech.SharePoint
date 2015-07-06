using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Queryable
{
	internal class SpQueryableList<TElement> : IOrderedQueryable<TElement>, IQueryProvider
	{
		public SpQueryableList(SPList list)
		{
			Guard.ThrowIfArgumentNull(list, "list");

			List = list;
			Expression = Expression.Constant(this);
		}

		protected SpQueryableList(SPList list, Expression expression)
		{
			Guard.ThrowIfArgumentNull(list, "list");
			Guard.ThrowIfArgumentNull(expression, "expression");

			if (!typeof(IQueryable<TElement>).IsAssignableFrom(expression.Type))
			{
				throw new ArgumentOutOfRangeException("expression");
			}

			List = list;
			Expression = expression;
		}

		public SPList List { get; private set; }
		public Expression Expression { get; private set; }

		public Type ElementType
		{
			get { return typeof (TElement); }
		}

		public IQueryProvider Provider
		{
			get { return this; }
		}

		public IEnumerator<TElement> GetEnumerator()
		{
			return Execute<IEnumerable<TElement>>(Expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Execute<IEnumerable>(Expression).GetEnumerator();
		}

		public IQueryable CreateQuery(Expression expression)
		{
			var elementType = TypeSystem.GetElementType(expression.Type);
			try
			{
				return (IQueryable)Activator.CreateInstance(typeof(SpQueryableList<>).MakeGenericType(elementType), this, expression);
			}
			catch (TargetInvocationException tie)
			{
				throw tie.InnerException;
			}
		}

		public IQueryable<TElement1> CreateQuery<TElement1>(Expression expression)
		{
			return new SpQueryableList<TElement1>(List, expression);
		}

		public object Execute(Expression expression)
		{
			return SpQueryContext.Execute(List, expression, false);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			var resultType = typeof (TResult);
			var isEnumerable = typeof(IEnumerable).IsAssignableFrom(resultType);

			return (TResult)SpQueryContext.Execute(List, expression, isEnumerable);
		}
	}
}