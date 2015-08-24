using System;
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data
{
	public abstract class SpDataContext<TDerived> where TDerived : SpDataContext<TDerived>
	{
		protected SpDataContext(ClientContext context)
		{

		}

		protected SpList<T> GetList<T>(Expression<Func<TDerived, SpList<T>>> listProperty)
		{
			throw new NotImplementedException();
		}
	}
}
