using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Server.Data.Mapper
{
	internal sealed class TypeMapper : TypeMapper<SPListItem>
	{
		public TypeMapper(MetaContentType contentType) : base(contentType)
		{
		}

		protected override void SetContentType(SPListItem spItem)
		{
			spItem[SPBuiltInFieldId.ContentTypeId] = new SPContentTypeId(ContentType.Id);
		}
	}
}