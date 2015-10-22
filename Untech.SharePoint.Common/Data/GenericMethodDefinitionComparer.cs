using System.Collections.Generic;
using System.Reflection;

namespace Untech.SharePoint.Common.Data
{
	internal class GenericMethodDefinitionComparer : IEqualityComparer<MethodInfo>
	{
		public static IEqualityComparer<MethodInfo> Comparer = new GenericMethodDefinitionComparer();

		public bool Equals(MethodInfo x, MethodInfo y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(MethodInfo obj)
		{
			if (obj == null) { return 0; }
			return obj.MetadataToken ^ obj.Module.GetHashCode();
		}
	}
}