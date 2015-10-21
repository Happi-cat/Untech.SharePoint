using System.Linq;

namespace Untech.SharePoint.Common.Data
{
	public interface ISpList<T> : IQueryable<T>
	{
		T Get(int id);

		void Add(T item);

		void Update(T item);

		void Delete(T item);
	}
}