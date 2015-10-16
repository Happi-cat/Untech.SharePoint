using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data
{
	internal class SpList<T> : SpLinqQuery<T>, ISpList<T>
	{
		public SpList(ISpListItemsProvider listItemsProvider)
			: base(FakeGetAll(listItemsProvider))
		{
			ListItemsProvider = listItemsProvider;
		}

		public ISpListItemsProvider ListItemsProvider { get; private set; }

		public void Add(T item)
		{
			ListItemsProvider.Add(item);
		}

		public void Update(T item)
		{
			ListItemsProvider.Update(item);
		}

		private static Expression FakeGetAll(ISpListItemsProvider listItemsProvider)
		{
			Guard.CheckNotNull("listItemsProvider", listItemsProvider);

			return SpQueryable.MakeFakeFetch(typeof(T), listItemsProvider);
		}
	}
}