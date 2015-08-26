using System.Linq;

namespace Untech.SharePoint.Client.Data
{
	public interface ISpList<T> : IQueryable<T>
	{
		IDataContext DataContext { get; }

		void Add(T item);

		void Update(T item);
	}
}