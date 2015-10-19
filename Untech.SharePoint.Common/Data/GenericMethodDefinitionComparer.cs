using System.Collections.Generic;
using System.Reflection;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data
{
	internal class GenericMethodDefinitionComparer : IEqualityComparer<MethodInfo>
	{
		public bool Equals(MethodInfo x, MethodInfo y)
		{
			return GetHashCode(x) == GetHashCode(y);
		}

		public int GetHashCode(MethodInfo obj)
		{
			Guard.CheckNotNull("obj", obj);
			if (obj.IsGenericMethod)
			{
				obj = obj.GetGenericMethodDefinition();
			}
			return obj.MetadataToken;
		}
	}
}