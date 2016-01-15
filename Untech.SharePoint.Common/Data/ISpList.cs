using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents interface for work with SP lists.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISpList<T> : IQueryable<T>
	{
		/// <summary>
		/// Gets item by id.
		/// </summary>
		/// <param name="id">Item id.</param>
		/// <returns>Entity with specified ID.</returns>
		T Get(int id);

		/// <summary>
		/// Adds new item to SP list.
		/// </summary>
		/// <param name="item">Entity to add.</param>
		T Add(T item);

		void Add(IEnumerable<T> items);

		/// <summary>
		/// Updates item with the specified ID in SP list.
		/// </summary>
		/// <param name="item">Item to update.</param>
		void Update(T item);

		void Update(IEnumerable<T> items);

		/// <summary>
		/// Deletes item with the specified ID from SP list.
		/// </summary>
		/// <param name="item">Item to delete.</param>
		void Delete(T item);

		void Delete(IEnumerable<T> items);
	}
}