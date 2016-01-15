using System.Collections.Generic;
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

		private ISpListItemsProvider ListItemsProvider { get; set; }

		public T Get(int id)
		{
			return ListItemsProvider.Get<T>(id);
		}

		public T Add(T item)
		{
			if (item == null) return default(T);
			return ListItemsProvider.Add(item);
		}

		public void Add(IEnumerable<T> items)
		{
			if (items == null) return;
			ListItemsProvider.Add(items);
		}

		public void Update(T item)
		{
			if (item == null) return;
			ListItemsProvider.Update(item);
		}

		public void Update(IEnumerable<T> items)
		{
			if (items == null) return;
			ListItemsProvider.Update(items);
		}

		public void Delete(T item)
		{
			if (item == null) return;
			ListItemsProvider.Delete(item);
		}

		public void Delete(IEnumerable<T> items)
		{
			if (items == null) return;
			ListItemsProvider.Delete(items);
		}

		private static Expression MakeFakeFetch(ISpListItemsProvider listItemsProvider)
		{
			Guard.CheckNotNull("listItemsProvider", listItemsProvider);

			return SpQueryable.MakeFakeFetch(typeof(T), listItemsProvider);
		}
	}
}