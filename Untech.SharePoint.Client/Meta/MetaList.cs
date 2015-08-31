using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Client.Meta.Providers;

namespace Untech.SharePoint.Client.Meta
{
	public class MetaList
	{
		public MetaList(string listTitle, SpFieldCollection fields, IMetaTypeProvider typeProvider)
		{
			ListTitle = listTitle;
			Fields = fields;
			ItemType = typeProvider.GetMetaType(this);
		}

		public string ListTitle { get; private set; }

		public SpFieldCollection Fields { get; private set; }

		public MetaType ItemType { get; private set; }

		public override string ToString()
		{
			return string.Format("( ListTitle='{0}'; ItemType={1}; )", ListTitle, ItemType);
		}
	}
}