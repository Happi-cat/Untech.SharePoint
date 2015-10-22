using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal sealed class TypeMapper : TypeMapper<ListItem>
	{
		public TypeMapper(MetaContentType contentType)
			: base(contentType)
		{

		}
	}
}