using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.SharePoint.Tools;

namespace Untech.SharePoint.Client.Extensions
{
	public static class EnumerableExtensions
	{
		public static string JoinToString<T>(this IEnumerable<T> enumerable, string delimeter = "; ")
		{
			return enumerable == null ? "" : string.Join(delimeter, enumerable);
		}

		public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			enumerable.ToList().ForEach(action);
		}
	}
}