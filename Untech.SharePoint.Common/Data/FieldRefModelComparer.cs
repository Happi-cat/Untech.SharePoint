using System.Collections.Generic;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Data
{
	internal class FieldRefModelComparer : IEqualityComparer<FieldRefModel>
	{
		public static IEqualityComparer<FieldRefModel> Comparer = new FieldRefModelComparer();

		public bool Equals(FieldRefModel x, FieldRefModel y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(FieldRefModel obj)
		{
			return MemberInfoComparer.Comparer.GetHashCode(obj.Member);
		}
	}
}