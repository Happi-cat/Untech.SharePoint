namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaList
	{
		private readonly MetaModel _model;

		protected MetaList(MetaModel model)
		{
			Guard.CheckNotNull("model", model);

			_model = model;
		}

		public MetaModel Model { get { return _model; } }

		public abstract string ListTitle { get; }

		public abstract SpFieldCollection Fields { get; }

		public abstract MetaType ItemType { get; }

		public override string ToString()
		{
			return string.Format("( ListTitle='{0}'; ItemType={1}; )", ListTitle, ItemType);
		}
	}
}