using System.Collections.Generic;
using Untech.SharePoint.Common.Data.QueryModels;

namespace Untech.SharePoint.Common.Data
{
	internal class MemberRefModelComparer : IEqualityComparer<MemberRefModel>
	{
		public static readonly IEqualityComparer<MemberRefModel> Default = new MemberRefModelComparer();

		public bool Equals(MemberRefModel x, MemberRefModel y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(MemberRefModel obj)
		{
			return obj == null ? 0 : MemberInfoComparer.Default.GetHashCode(obj.Member);
		}
	}
}