using System;

namespace Untech.SharePoint.Common.Mappings
{
	public interface IMappingSourceResolver
	{
		IMappingSource Resolve(Type contextType);
	}
}