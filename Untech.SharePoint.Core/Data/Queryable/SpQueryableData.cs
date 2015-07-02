using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Caml;

namespace Untech.SharePoint.Core.Data.Queryable
{
	internal class SpQueryableData<TElement> : IOrderedQueryable<TElement>, ISpModelContext
	{
		public SpQueryableData(SPList list)
		{
			Guard.ThrowIfArgumentNull(list, "list");

			List = list;
			Provider = new SpQueryProdiver();
			Expression = Expression.Constant(this);
		}

		protected internal SpQueryableData(SPList list, SpQueryProdiver provider, Expression expression)
		{
			Guard.ThrowIfArgumentNull(list, "list");
			Guard.ThrowIfArgumentNull(provider, "provider");
			Guard.ThrowIfArgumentNull(expression, "expression");
			
			if (!typeof(IQueryable<TElement>).IsAssignableFrom(expression.Type))
			{
				throw new ArgumentOutOfRangeException("expression");
			}

			List = list;
			Provider = provider;
			Expression = expression;
		}

		protected internal SPList List { get; private set; }

		public Expression Expression { get; private set; }

		public Type ElementType
		{
			get { return typeof(TElement); }
		}

		public IQueryProvider Provider { get; private set; }

		public IEnumerator<TElement> GetEnumerator()
		{
			return (Provider.Execute<IEnumerable<TElement>>(Expression)).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (Provider.Execute<IEnumerable>(Expression)).GetEnumerator();
		}

		
		public string GetSpFieldInternalName(Type modelType, string propertyOrFieldName)
		{
			throw new NotImplementedException();
		}

		public string GetSpFieldTypeAsString(Type modelType, string propertyOrFieldName)
		{
			throw new NotImplementedException();
		}

		public object ConvertToSpValue(Type modelType, string propertyOrFieldName, object value)
		{
			throw new NotImplementedException();
		}
	}
}