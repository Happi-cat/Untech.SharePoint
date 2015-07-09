using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Data.Queryable;

namespace Untech.SharePoint.Core.Extensions
{
	public static class SPListExtensions
	{
		public static IQueryable<TElement> AsQueryable<TElement>(this SPList list)
		{
			Guard.ThrowIfArgumentNull(list, "list");

			return new SpQueryableList<TElement>(list);
		}
	}
}