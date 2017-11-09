using Microsoft.SharePoint.Client;
using Untech.SharePoint.Data.Mapper;
using Untech.SharePoint.MetaModels;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal sealed class TypeMapper : TypeMapper<ListItem>
	{
		public TypeMapper(MetaContentType contentType)
			: base(contentType)
		{
		}

		protected override void SetContentType(ListItem spItem)
		{
			spItem["ContentTypeId"] = ContentType.Id;
		}
	}
}