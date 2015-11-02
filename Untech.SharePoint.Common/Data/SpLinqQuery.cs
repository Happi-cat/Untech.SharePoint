using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	internal class SpLinqQuery<T> : IOrderedQueryable<T>
	{
		public SpLinqQuery(Expression expression)
		{
			Guard.CheckNotNull("expression", expression);
			Guard.CheckIsTypeAssignableTo<IQueryable<T>>("expression", expression.Type);

			Expression = expression;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Expression Expression { get; private set; }
		public Type ElementType { get { return typeof(T); } }
		public IQueryProvider Provider { get { return SpLinqQueryProvider.Instance; } }
	}
}