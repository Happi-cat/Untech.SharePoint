using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Services;

namespace Untech.SharePoint.Common.Configuration
{
	public class MappingSources
	{
		public IMappingSource<T> Annotated<T>()
			where T: ISpContext
		{
			return new AnnotationMapping.AnnotatedMappingSource<T>();
		}
	}
}