namespace Untech.SharePoint.Client.Data
{
	interface ISpList<T>
	{
		void Add(T item);

		void Update(T item);
	}
}