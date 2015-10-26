using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Server.Data.Mapper;

namespace Untech.SharePoint.Server.MetaModels.Visitors
{
	internal class MapperInitializer : BaseMetaModelVisitor
	{
		public override void VisitContentType(MetaContentType contentType)
		{
			contentType.SetMapper(new TypeMapper<SPListItem>(contentType));

			base.VisitContentType(contentType);
		}

		public override void VisitField(MetaField field)
		{
			field.SetMapper(new FieldMapper<SPListItem>(field, new StoreAccessor(field)));
		}
		
	}
}