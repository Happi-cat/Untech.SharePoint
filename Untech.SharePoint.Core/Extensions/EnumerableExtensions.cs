using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Core.Extensions
{
	public static class EnumerableExtensions
	{
		public static string JoinToString<T>(this IEnumerable<T> enumerable, string delimeter = "; ")
		{
			return enumerable == null ? null : enumerable.Aggregate("", (str, n) => str + n + delimeter);
		}
	}
}