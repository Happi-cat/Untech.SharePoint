using System.Linq;

namespace Untech.SharePoint.Client.Data
{
	public interface ISpList<T> : IQueryable<T>
	{
		BaseDataContext DataContext { get; }

		void Add(T item);

		void Update(T item);
	}
}