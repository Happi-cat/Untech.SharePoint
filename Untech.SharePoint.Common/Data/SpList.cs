using System.Linq.Expressions;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	internal class SpList<T> : SpLinqQuery<T>, ISpList<T>
	{
		public SpList(ISpListItemsProvider listItemsProvider)
			: base(MakeFakeFetch(listItemsProvider))
		{
			ListItemsProvider = listItemsProvider;
		}

		public ISpListItemsProvider ListItemsProvider { get; private set; }

		public T Get(int id)
		{
			return ListItemsProvider.Get<T>(id);
		}

		public void Add(T item)
		{
			ListItemsProvider.Add(item);
		}

		public void Update(T item)
		{
			ListItemsProvider.Update(item);
		}

		public void Delete(T item)
		{
			ListItemsProvider.Delete(item);
		}

		private static Expression MakeFakeFetch(ISpListItemsProvider listItemsProvider)
		{
			Guard.CheckNotNull("listItemsProvider", listItemsProvider);

			return SpQueryable.MakeFakeFetch(typeof(T), listItemsProvider);
		}
	}
}