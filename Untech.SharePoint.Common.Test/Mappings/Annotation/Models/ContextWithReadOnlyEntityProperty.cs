using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class ContextWithReadOnlyEntityProperty : AnnotatedContext
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithReadOnlyProperty> InvalidEntity { get; set; }
	}
}