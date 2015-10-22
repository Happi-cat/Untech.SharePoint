using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal sealed class FieldMapper : FieldMapper<ListItem>
	{
		public FieldMapper(MetaField field)
			:base (field)
		{

		}

		protected override object GetStoreValue(ListItem spItem)
		{
			return spItem[Field.InternalName];
		}

		protected override void SetStoreValue(ListItem spItem, object value)
		{
			spItem[Field.InternalName] = value;
		}
	}
}