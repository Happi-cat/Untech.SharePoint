using System.Collections.Generic;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Services
{
	public interface IListService
	{
		IEnumerable<TEntity> GetItems<TEntity>(MetaContentType listContentType);

		void AddItem<TEntity>(MetaContentType listContentType, TEntity item);

		void UpdateItem<TEntity>(MetaContentType listContentType, TEntity item);
	}
}