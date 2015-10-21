using System;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Data;
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

		protected override object GetStoreValue(ListItem item)
		{
			return item[Field.InternalName];
		}

		protected override void SetStoreValue(ListItem item, object value)
		{
			item[Field.InternalName] = value;
		}
	}
}