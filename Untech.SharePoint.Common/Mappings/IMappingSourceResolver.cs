using System;

namespace Untech.SharePoint.Common.Mappings
{
	public interface IMappingSourceResolver
	{
		bool CanResolve(Type contextType);

		IMappingSource Resolve(Type contextType);
	}
}