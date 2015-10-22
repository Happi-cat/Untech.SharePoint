using Untech.SharePoint.Common.Data.Mapper;

namespace Untech.SharePoint.Common.MetaModels
{
	public static class MetaModelExtensions
	{
		private const string MapperProperty = "Mapper";

		public static FieldMapper<TSPItem> GetMapper<TSPItem>(this MetaField field)
		{
			return field.GetAdditionalProperty<FieldMapper<TSPItem>>(MapperProperty);
		}
		public static TypeMapper<TSPItem> GetMapper<TSPItem>(this MetaContentType contentType)
		{
			return contentType.GetAdditionalProperty<TypeMapper<TSPItem>>(MapperProperty);
		}

		public static void SetMapper<TSPItem>(this MetaField field, FieldMapper<TSPItem> mapper)
		{
			field.SetAdditionalProperty(MapperProperty, mapper);
		}
		public static void SetMapper<TSPItem>(this MetaContentType contentType, TypeMapper<TSPItem> mapper)
		{
			contentType.SetAdditionalProperty(MapperProperty, mapper);
		}

	}
}