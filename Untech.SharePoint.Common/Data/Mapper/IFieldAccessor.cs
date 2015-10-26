namespace Untech.SharePoint.Common.Data.Mapper
{
	public interface IFieldAccessor<in T>
	{
		bool CanGetValue { get; }

		bool CanSetValue { get; }

		object GetValue(T instance);

		void SetValue(T instance, object value);
	}
}