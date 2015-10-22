using System.Collections.Generic;
using System.Reflection;
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

	internal class MemberInfoComparer : IEqualityComparer<MemberInfo>
	{
		public static IEqualityComparer<MemberInfo> Comparer = new MemberInfoComparer();

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
