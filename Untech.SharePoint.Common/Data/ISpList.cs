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

		/// <summary>
		/// Updates item with the specified ID in SP list.
		/// </summary>
		/// <param name="item">Item to update.</param>
		T Update(T item);

		/// <summary>
		/// Deletes item with the specified ID from SP list.
		/// </summary>
		/// <param name="item">Item to delete.</param>
		void Delete(T item);
	}
}