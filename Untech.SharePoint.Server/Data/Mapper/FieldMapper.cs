using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Server.Data.Mapper
{
	internal sealed class FieldMapper : FieldMapper<SPListItem>
	{
		public FieldMapper(MetaField field)
			: base(field)
		{
		}

		protected override object GetStoreValue(SPListItem spItem)
		{
			return spItem[Field.InternalName];
		}

		protected override void SetStoreValue(SPListItem spItem, object value)
		{
			spItem[Field.InternalName] = value;
		}
	}
}