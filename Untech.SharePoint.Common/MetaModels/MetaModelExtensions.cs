using Untech.SharePoint.Common.Data.Mapper;

namespace Untech.SharePoint.Common.MetaModels
{
	public static class MetaModelExtensions
	{
		private const string MapperProperty = "Mapper";

		public static FieldMapper<TListItem> GetMapper<TListItem>(this MetaField field)
		{
			return field.GetAdditionalProperty<FieldMapper<TListItem>>(MapperProperty);
		}
		public static TypeMapper<TListItem> GetMapper<TListItem>(this MetaContentType contentType)
		{
			return contentType.GetAdditionalProperty<TypeMapper<TListItem>>(MapperProperty);
		}

		public static void SetMapper<TListItem>(this MetaField field, FieldMapper<TListItem> mapper)
		{
			field.SetAdditionalProperty(MapperProperty, mapper);
		}
		public static void SetMapper<TListItem>(this MetaContentType contentType, TypeMapper<TListItem> mapper)
		{
			contentType.SetAdditionalProperty(MapperProperty, mapper);
		}

	}
}