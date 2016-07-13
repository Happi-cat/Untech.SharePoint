using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal sealed class StoreAccessor : StoreAccessor<ListItem>
	{
		public StoreAccessor(MetaField field)
			: base(field)
		{
		}

		public override object GetValue(ListItem instance)
		{
			return instance[Field.InternalName];
		}

		public override void SetValue(ListItem instance, object value)
		{
			instance[Field.InternalName] = value;
		}
	}
}