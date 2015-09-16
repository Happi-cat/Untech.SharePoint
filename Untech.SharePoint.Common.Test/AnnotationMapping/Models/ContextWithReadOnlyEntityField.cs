using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class ContextWithReadOnlyEntityField : TestContext
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithReadOnlyField> InvalidEntity { get; set; }
	}
}