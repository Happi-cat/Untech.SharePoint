using System.Collections.Generic;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents interface of SP list data accessor and items provider.
	/// </summary>
	public interface ISpListItemsProvider
	{
		/// <summary>
		/// Gets list associated with this instance of the <see cref="ISpListItemsProvider"/>.
		/// </summary>
		MetaList List { get; }

		/// <summary>
		/// Fetchs items by the specified CAML query string.
		/// </summary>
		/// <typeparam name="T">Type of element to fetch.</typeparam>
		/// <param name="caml">CAML query string in format: <example><![CDATA[<View><Query><Where></Where></Query></View>]]></example></param>
		/// <returns>Collection of loaded items.</returns>
		IEnumerable<T> Fetch<T>(string caml);

		/// <summary>
		/// Determines whether a sequence returned by CAML query contains any elements.
		/// </summary>
		/// <param name="caml">CAML query string in format: <example><![CDATA[<View><Query><Where></Where></Query></View>]]></example></param>
		/// <returns>true if the returned sequence contains any elements; otherwise, false.</returns>
		bool Any(string caml);

		/// <summary>
		/// Returns the number of elements in a sequence return by CAML query.
		/// </summary>
		/// <param name="caml">CAML query string in format: <example><![CDATA[<View><Query><Where></Where></Query></View>]]></example></param>
		/// <returns>The number of element in the returned sequence.</returns>
		int Count(string caml);

		/// <summary>
		/// Returns the only element of a sequence returned by CAML query, or a default value if the sequence is empty;
		/// </summary>
		/// <typeparam name="T">Type of element to fetch.</typeparam>
		/// <param name="caml">CAML query string in format: <example><![CDATA[<View><Query><Where></Where></Query></View>]]></example></param>
		/// <returns>The single element of the retuned sequence, or default(<typeparamref name="T"/>) if the sequense conatins no elements.</returns>
		T SingleOrDefault<T>(string caml);

		/// <summary>
		/// Returns the first element in a sequence returned by CAML query or a default value if the sequence is empty.
		/// </summary>
		/// <typeparam name="T">Type of element to fetch.</typeparam>
		/// <param name="caml">CAML query string in format: <example><![CDATA[<View><Query><Where></Where></Query></View>]]></example></param>
		/// <returns>The first element of the retuned sequence, or default(<typeparamref name="T"/>) if the sequense conatins no elements.</returns>
		T FirstOrDefault<T>(string caml);

		/// <summary>
		/// Returns the element at a specified index in a sequence returned by CAML query or a default value if the index is out of range.
		/// </summary>
		/// <typeparam name="T">Type of element to fetch.</typeparam>
		/// <param name="caml">CAML query string in format: <example><![CDATA[<View><Query><Where></Where></Query></View>]]></example></param>
		/// <param name="index"></param>
		/// <returns>The element at specified index in the retuned sequence, or default(<typeparamref name="T"/>) if the sequense conatins no elements.</returns>
		T ElementAtOrDefault<T>(string caml, int index);

		/// <summary>
		/// Gets item by id.
		/// </summary>
		/// <typeparam name="T">Type of entity to fetch.</typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		T Get<T>(int id);

		/// <summary>
		/// Adds new item to SP list.
		/// </summary>
		/// <typeparam name="T">Type of entity to add.</typeparam>
		/// <param name="item"></param>
		void Add<T>(T item);

		/// <summary>
		/// Updates item with the specified ID in SP list.
		/// </summary>
		/// <typeparam name="T">Type of entity to update.</typeparam>
		/// <param name="item"></param>
		void Update<T>(T item);

		/// <summary>
		/// Deletes item with the specified ID from SP list.
		/// </summary>
		/// <typeparam name="T">Type of entity to delete.</typeparam>
		/// <param name="item"></param>
		void Delete<T>(T item);
	}
}