using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal class MapperInitializer : BaseMetaModelVisitor
	{
		public override void VisitContentType(MetaContentType contentType)
		{
			contentType.SetMapper(new TypeMapper(contentType));

			base.VisitContentType(contentType);
		}

		public override void VisitField(MetaField field)
		{
			field.SetMapper(new FieldMapper<ListItem>(field, new StoreAccessor(field)));
		}
	}
}