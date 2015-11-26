using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Extensions
{
	/// <summary>
	/// Provide a set of static methods for work with <see cref="IEnumerable{T}"/>.
	/// </summary>
	[PublicAPI]
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Joins enumerable into single string.
		/// </summary>
		/// <typeparam name="T">The type of collection element.</typeparam>
		/// <param name="enumerable">Collection to join.</param>
		/// <param name="delimeter">Item delimiter.</param>
		/// <returns>Concatenated string</returns>
		[NotNull]
		public static string JoinToString<T>([CanBeNull]this IEnumerable<T> enumerable, [CanBeNull]string delimeter = "; ")
		{
			return enumerable == null ? "" : string.Join(delimeter ?? "", enumerable);
		}

		/// <summary>
		/// Iterates through the specified <paramref name="enumerable"/> and invokes <paramref name="action"/> for each element.
		/// </summary>
		/// <typeparam name="T">The type of collection element.</typeparam>
		/// <param name="enumerable">Collection to iterate over.</param>
		/// <param name="action">Action that should be invoke for every item in the specified collection.</param>
		public static void Each<T>([CanBeNull]this IEnumerable<T> enumerable, [NotNull]Action<T> action)
		{
			Guard.CheckNotNull("action", action);
			if (enumerable == null) return;

			foreach (var item in enumerable)
			{
				action(item);
			}
		}

		/// <summary>
		/// Determines whether the specified <paramref name="enumerable"/> is null or empty.
		/// </summary>
		/// <typeparam name="T">The type of collection element.</typeparam>
		/// <param name="enumerable">Collectino that should be checked.</param>
		/// <returns>true if the <see cref="IEnumerable{T}"/> is null or empty; otherwise, false.</returns>
		[ContractAnnotation("null => true")]
		public static bool IsNullOrEmpty<T>([CanBeNull]this IEnumerable<T> enumerable)
		{
			return enumerable == null || !enumerable.Any();
		}

		/// <summary>
		/// Returns empty enumerable or <paramref name="enumerable"/>.
		/// </summary>
		/// <param name="enumerable">Enumerable to return.</param>
		/// <typeparam name="T">The type of collection element.</typeparam>
		/// <returns>Empty Enumerable if <paramref name="enumerable"/> is null; otherwise, <paramref name="enumerable"/>.</returns>
		[NotNull]
		public static IEnumerable<T> EmptyIfNull<T>([CanBeNull][NoEnumeration] this IEnumerable<T> enumerable)
		{
			return enumerable ?? Enumerable.Empty<T>();
		}
	}
}