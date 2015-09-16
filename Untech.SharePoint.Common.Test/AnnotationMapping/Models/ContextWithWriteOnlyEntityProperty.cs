using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class ContextWithWriteOnlyEntityProperty : TestContext
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithWriteOnlyProperty> InvalidEntity { get; set; }
	}
}