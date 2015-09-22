using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class ContextWithEntityIndexer : TestContext
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithIndexer> InvalidEntity { get; set; }
	}
}