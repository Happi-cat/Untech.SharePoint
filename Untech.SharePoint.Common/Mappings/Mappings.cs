using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Mappings
{
	public sealed class Mappings
	{
		public IMappingSource<TContext> Annotated<TContext>()
			where TContext : ISpContext
		{
			return new AnnotatedMappingSource<TContext>();
		}
	}
}