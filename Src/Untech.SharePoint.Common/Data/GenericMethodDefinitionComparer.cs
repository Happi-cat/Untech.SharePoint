using System.Collections.Generic;
using System.Reflection;

namespace Untech.SharePoint.Common.Data
{
	internal class GenericMethodDefinitionComparer : IEqualityComparer<MethodInfo>
	{
		public static readonly IEqualityComparer<MethodInfo> Default = new GenericMethodDefinitionComparer();

		public bool Equals(MethodInfo x, MethodInfo y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(MethodInfo obj)
		{
			if (obj == null)
			{
				return 0;
			}

			unchecked
			{
				var hash = 17;
				hash = hash * 37 + obj.MetadataToken;
				hash = hash * 37 + obj.Module.GetHashCode();
				return hash;
			}
		}
	}
}