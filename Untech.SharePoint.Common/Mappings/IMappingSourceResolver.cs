using System;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Mappings
{
	public interface IMappingSourceResolver
	{
		IMappingSource Resolve<TContext>()
			where TContext : ISpContext;

		IMappingSource Resolve(Type contextType);
	}
}