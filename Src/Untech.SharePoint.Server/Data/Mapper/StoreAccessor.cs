using Microsoft.SharePoint;
using Untech.SharePoint.Data.Mapper;
using Untech.SharePoint.MetaModels;

namespace Untech.SharePoint.Server.Data.Mapper
{
	internal sealed class StoreAccessor : StoreAccessor<SPListItem>
	{
		public StoreAccessor(MetaField field) : base(field)
		{
		}

		public override object GetValue(SPListItem instance)
		{
			return instance[Field.InternalName];
		}

		public override void SetValue(SPListItem instance, object value)
		{
			instance[Field.InternalName] = value;
		}
	}
}