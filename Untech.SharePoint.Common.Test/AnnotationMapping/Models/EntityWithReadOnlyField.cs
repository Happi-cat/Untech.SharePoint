using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class EntityWithReadOnlyField : TestEntity
	{
		[SpField]
		public readonly string ReadonlyField = "Test";
	}
}