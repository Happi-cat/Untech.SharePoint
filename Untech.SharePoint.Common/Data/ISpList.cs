using System.Linq;

namespace Untech.SharePoint.Common.Data
{
	public interface ISpList<T> : IOrderedQueryable<T>
	{
		void Add(T item);

		void Update(T item);
	}
}