using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data
{
	internal class SpList<T> : SpLinqQuery<T>, ISpList<T>
	{
		public SpList(ISpItemsProvider itemsProvider)
			: base(FakeGetAll(itemsProvider))
		{
			ItemsProvider = itemsProvider;
		}

		public ISpItemsProvider ItemsProvider { get; private set; }

		public void Add(T item)
		{
			ItemsProvider.Add(item);
		}

		public void Update(T item)
		{
			ItemsProvider.Update(item);
		}

		private static Expression FakeGetAll(ISpItemsProvider itemsProvider)
		{
			Guard.CheckNotNull("itemsProvider", itemsProvider);

			return SpQueryable.MakeFakeGetAll(typeof(T), itemsProvider);
		}
	}
}