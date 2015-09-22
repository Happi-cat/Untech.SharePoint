using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class ContextWithReadOnlyEntityField : AnnotatedContext
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithReadOnlyField> InvalidEntity { get; set; }
	}
}