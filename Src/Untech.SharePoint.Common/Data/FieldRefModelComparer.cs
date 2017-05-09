using System.Collections.Generic;
using Untech.SharePoint.Data.QueryModels;

namespace Untech.SharePoint.Data
{
	internal class FieldRefModelComparer : IEqualityComparer<FieldRefModel>
	{
		public static readonly IEqualityComparer<FieldRefModel> Default = new FieldRefModelComparer();

		public bool Equals(FieldRefModel x, FieldRefModel y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;
			if (x.Type != y.Type) return false;
			return x.Type != FieldRefType.KnownMember || MemberRefModelComparer.Default.Equals((MemberRefModel)x, (MemberRefModel)y);
		}

		public int GetHashCode(FieldRefModel obj)
		{
			if (obj == null) return 0;
			unchecked
			{
				var hash = 17;
				hash = hash * 37 + obj.Type.GetHashCode();
				if (obj.Type == FieldRefType.KnownMember)
				{
					hash = hash * 37 + MemberRefModelComparer.Default.GetHashCode((MemberRefModel)obj);
				}
				return hash;
			}
		}
	}
}