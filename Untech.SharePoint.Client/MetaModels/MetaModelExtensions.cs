using Untech.SharePoint.Client.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.MetaModels
{
	internal static class MetaModelExtensions
	{
		private const string MapperProperty = "Mapper";

		internal static FieldMapper GetMapper(this MetaField field)
		{
			return field.GetAdditionalProperty<FieldMapper>(MapperProperty);
		}
		internal static TypeMapper GetMapper(this MetaContentType contentType)
		{
			return contentType.GetAdditionalProperty<TypeMapper>(MapperProperty);
		}

		internal static void SetMapper(this MetaField field, FieldMapper mapper)
		{
			field.SetAdditionalProperty(MapperProperty, mapper);
		}
		internal static void SetMapper(this MetaContentType contentType, TypeMapper mapper)
		{
			contentType.SetAdditionalProperty(MapperProperty, mapper);
		}

	}
}