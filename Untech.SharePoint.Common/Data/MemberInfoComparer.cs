using System.Collections.Generic;
using System.Reflection;

namespace Untech.SharePoint.Common.Data
{
	internal class MemberInfoComparer : IEqualityComparer<MemberInfo>
	{
		public static readonly IEqualityComparer<MemberInfo> Default = new MemberInfoComparer();

		public bool Equals(MemberInfo x, MemberInfo y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;
			return x.MetadataToken == y.MetadataToken &&
				x.Module == y.Module;
		}

		public int GetHashCode(MemberInfo obj)
		{
			if (obj == null) return 0;
			unchecked
			{
				var hash = 17;
				hash = hash*37 + obj.MetadataToken;
				hash = hash*37 + obj.Module.GetHashCode();
				return hash;
			}
		}
	}
}
