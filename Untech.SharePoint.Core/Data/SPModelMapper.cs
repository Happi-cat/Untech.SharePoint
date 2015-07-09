using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data
{
	public static class SpModelMapper
	{
		public static void Map<T>(T sourceItem, SPListItem destItem)
		{
			var model = MetaModelPool.Instance.Get<T>();
			
			model.Mapper.Map(sourceItem, destItem);
		}

		public static void Map(object sourceItem, SPListItem destItem)
		{
			var model = MetaModelPool.Instance.Get(sourceItem.GetType());

			model.Mapper.Map(sourceItem, destItem);
		}

		public static void Map<T>(SPListItem sourceItem, T destItem)
		{
			var model = MetaModelPool.Instance.Get<T>();

			model.Mapper.Map(sourceItem, destItem);
		}

		public static void Map(SPListItem sourceItem, object destItem)
		{
			var model = MetaModelPool.Instance.Get(sourceItem.GetType());

			model.Mapper.Map(sourceItem, destItem);
		}
	}
}