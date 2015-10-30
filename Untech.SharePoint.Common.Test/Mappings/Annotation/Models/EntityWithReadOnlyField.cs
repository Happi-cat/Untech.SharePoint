using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class EntityWithReadOnlyField : Entity
	{
		[SpField]
		public readonly string ReadonlyField = "Test";
	}
}