using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Core.Extensions
{
	public static class ObjectExtensions
	{
		public static bool In<T>(this T obj, IEnumerable<T> collection)
		{
			return collection != null && collection.Contains(obj);
		}
	}
}