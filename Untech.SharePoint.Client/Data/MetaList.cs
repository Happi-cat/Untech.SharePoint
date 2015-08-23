namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaList
	{
		public abstract MetaModel Model { get; }

		public abstract string ListTitle { get; }

		public abstract SpFieldCollection Fields { get; }

		public abstract MetaType ItemType { get; }
	}
}