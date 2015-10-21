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

		protected override object GetStoreValue(SPListItem item)
		{
			return item[Field.InternalName];
		}

		protected override void SetStoreValue(SPListItem item, object value)
		{
			item[Field.InternalName] = value;
		}
	}
}