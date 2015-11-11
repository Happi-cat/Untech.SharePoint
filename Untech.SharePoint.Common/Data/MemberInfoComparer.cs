using System.Collections.Generic;
using System.Reflection;

namespace Untech.SharePoint.Common.Data
{
	internal class MemberInfoComparer : IEqualityComparer<MemberInfo>
	{
		public static readonly IEqualityComparer<MemberInfo> Default = new MemberInfoComparer();

		public bool Equals(MemberInfo x, MemberInfo y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(MemberInfo obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.MetadataToken ^ obj.Module.GetHashCode();
		}
	}
}
