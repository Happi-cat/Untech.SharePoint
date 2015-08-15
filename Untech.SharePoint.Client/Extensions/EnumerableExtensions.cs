using System.Collections.Generic;

namespace Untech.SharePoint.Client.Extensions
{
	public static class EnumerableExtensions
	{
		public static string JoinToString<T>(this IEnumerable<T> enumerable, string delimeter = "; ")
		{
			return string.Join(delimeter, enumerable);
		}
	}
}