using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class ContextWithEntityIndexer : TestContext
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithIndexer> InvalidEntity { get; set; }
	}
}