using System;
using Untech.SharePoint.Common.Collections;

namespace Untech.SharePoint.Common.Mappings
{
	public class MappingSourceContainer : Container<Type, IMappingSource>, IMappingSourceResolver
	{
		public bool CanResolve(Type contextType)
		{
			return IsRegistered(contextType);
		}
	}
}