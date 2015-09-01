using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Client.Meta.Providers;

namespace Untech.SharePoint.Client.Mapping
{
	public class ContextMap<T>
	{
		public ListMap List<TEntity>(Expression<Func<T, ISpList<TEntity>>> listExpression, string listTitle) 
		{
			throw new NotImplementedException();
		}

		public ListMap List<TEntity, TProvider>(Expression<Func<T, ISpList<TEntity>>> listExpression) 
			where TProvider : IMetaListProvider, new()
		{
			throw new NotImplementedException();
		}

		public ListMap List<TEntity>(Expression<Func<T, ISpList<TEntity>>> listExpression, IMetaListProvider listProvider) 
		{
			throw new NotImplementedException();
		}
	}
}
