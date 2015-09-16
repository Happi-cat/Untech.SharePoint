using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class ContextWithReadOnlyEntityProperty : TestContext
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithReadOnlyProperty> InvalidEntity { get; set; }
	}
}