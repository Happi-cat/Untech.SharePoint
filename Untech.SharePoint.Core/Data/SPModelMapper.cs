using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data
{
	public static class SPModelMapper
	{
		public static void Map<T>(T sourceItem, SPListItem destItem)
		{
			var mapper = DataMapperPool.Instance.Get<T>();

			mapper.Map(sourceItem, destItem);
		}

		public static void Map(object sourceItem, SPListItem destItem)
		{
			var mapper = DataMapperPool.Instance.Get(sourceItem.GetType());

			mapper.Map(sourceItem, destItem);
		}

		public static void Map<T>(SPListItem sourceItem, T destItem)
		{
			var mapper = DataMapperPool.Instance.Get<T>();

			mapper.Map(sourceItem, destItem);
		}

		public static void Map(SPListItem sourceItem, object destItem)
		{
			var mapper = DataMapperPool.Instance.Get(sourceItem.GetType());

			mapper.Map(sourceItem, destItem);
		}
	}
}