using System.Collections.Generic;
using Untech.SharePoint.Data.QueryModels;

namespace Untech.SharePoint.Data
{
	internal class MemberRefModelComparer : IEqualityComparer<MemberRefModel>
	{
		public static readonly IEqualityComparer<MemberRefModel> Default = new MemberRefModelComparer();

		public bool Equals(MemberRefModel x, MemberRefModel y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;
			return MemberInfoComparer.Default.Equals(x.Member, y.Member);
		}

		public int GetHashCode(MemberRefModel obj)
		{
			return obj == null ? 0 : MemberInfoComparer.Default.GetHashCode(obj.Member);
		}
	}
}