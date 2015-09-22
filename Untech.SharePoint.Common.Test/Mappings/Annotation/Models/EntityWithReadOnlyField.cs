using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class EntityWithReadOnlyField : AnnotatedEntity
	{
		[SpField]
		public readonly string ReadonlyField = "Test";
	}
}