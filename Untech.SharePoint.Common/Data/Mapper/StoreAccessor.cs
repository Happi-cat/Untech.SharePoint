using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data.Mapper
{
	public abstract class StoreAccessor<TSPItem> : IFieldAccessor<TSPItem>
	{
		protected StoreAccessor(MetaField field)
		{
			Field = field;
		}

		public MetaField Field { get; private set; }

		public bool CanGetValue { get { return true; } }
		public bool CanSetValue { get { return !Field.ReadOnly && !Field.IsCalculated; } }
		public abstract object GetValue(TSPItem instance);

		public abstract void SetValue(TSPItem instance, object value);
	}
}