namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaAccessor<T>
	{
		protected MetaAccessor(MetaDataMember member) 
		{
			Guard.CheckNotNull("member", member);

			DataMember = member;
		}
		
		public MetaDataMember DataMember { get; private set; }

		public virtual bool CanRead { get { return true; } }

		public virtual bool CanWrite { get { return true; } }
		
		public abstract object GetValue(T instance);

		public abstract void SetValue(T instance, object value);
	}
}