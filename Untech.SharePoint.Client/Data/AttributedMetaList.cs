using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class AttributedMetaList : MetaList
	{
		private readonly string _listTitle;
		private readonly MetaType _itemType;
		private readonly SpFieldCollection _fields;

		public AttributedMetaList(MetaModel model, SpListAttribute listAttr, Type itemType, ClientContext context)
			: base(model)
		{
			_listTitle = listAttr.ListTitle;
			_itemType = new AttributedMetaType(model, this, itemType);
			_fields = new SpFieldCollection(context.GetListFields(_listTitle));
		}

		public override string ListTitle
		{
			get { return _listTitle; }
		}

		public override SpFieldCollection Fields { get { return _fields; } }

		public override MetaType ItemType
		{
			get { return _itemType; }
		}
	}

}