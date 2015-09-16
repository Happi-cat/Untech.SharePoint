using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Services;

namespace Untech.SharePoint.Common.Configuration
{
	public sealed class MappingSources
	{
		public IMappingSource<TContext> Annotated<TContext>()
			where TContext: ISpContext
		{
			return new AnnotatedMappingSource<TContext>();
		}
	}
}