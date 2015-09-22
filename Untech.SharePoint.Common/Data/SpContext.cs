using System;

namespace Untech.SharePoint.Common.Data
{
	public abstract class SpContext : ISpContext
	{
		protected ISpList<TEntity> GetList<TEntity>(Func<ISpContext, TEntity> listAccessor)
		{
			throw new NotImplementedException();
		}
	}
}