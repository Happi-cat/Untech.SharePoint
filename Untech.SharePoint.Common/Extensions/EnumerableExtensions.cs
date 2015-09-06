using System;
using System.Collections.Generic;

namespace Untech.SharePoint.Common.Extensions
{
	public static class EnumerableExtensions
	{
		public static string JoinToString<T>(this IEnumerable<T> enumerable, string delimeter = "; ")
		{
			return enumerable == null ? "" : string.Join(delimeter, enumerable);
		}

		public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}
	}
}