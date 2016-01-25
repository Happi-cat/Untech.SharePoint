using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents interface for work with SP lists.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public interface ISpList<T> : IQueryable<T>
	{
		string Title { get; }

		/// <summary>
		/// Gets item by id.
		/// </summary>
		/// <param name="id">Item id.</param>
		/// <returns>Entity with specified ID.</returns>
		T Get(int id);

		/// <summary>
		/// Adds new item to SP list.
		/// </summary>
		/// <param name="item">Item to add.</param>
		T Add(T item);

		/// <summary>
		/// Adds new items to SP list.
		/// </summary>
		/// <param name="items">Items to add.</param>
		void Add(IEnumerable<T> items);

		/// <summary>
		/// Updates item with the specified ID in SP list.
		/// </summary>
		/// <param name="item">Item to update.</param>
		T Update(T item);

		/// <summary>
		/// Updates items with the specified IDs in SP list.
		/// </summary>
		/// <param name="items">Items to update.</param>
		void Update(IEnumerable<T> items);

		/// <summary>
		/// Deletes item with the specified ID from SP list.
		/// </summary>
		/// <param name="item">Item to delete.</param>
		void Delete(T item);

		/// <summary>
		/// Deletes items with the specified IDs from SP list.
		/// </summary>
		/// <param name="items">Items to delete.</param>
		void Delete(IEnumerable<T> items);
	}
}