using System;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class AttributedMetaList : MetaList
	{
		private readonly string _listTitle;
		private readonly MetaType _itemType;
		private readonly SpFieldCollection _fields;

		public AttributedMetaList(MetaModel model, SpListAttribute listAttr, Type itemType, ISpFieldsResolver resolver)
			: base(model)
		{
			Guard.CheckNotNull("listAttr", listAttr);
			Guard.CheckNotNull("itemType", itemType);
			Guard.CheckNotNull("resolver", resolver);

			_listTitle = listAttr.ListTitle;
			_itemType = new AttributedMetaType(model, this, itemType);
			_fields = resolver.GetFields(_listTitle);
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