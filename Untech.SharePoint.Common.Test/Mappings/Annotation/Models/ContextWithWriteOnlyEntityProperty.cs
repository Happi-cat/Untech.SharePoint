using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class ContextWithWriteOnlyEntityProperty : AnnotatedContext
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithWriteOnlyProperty> InvalidEntity { get; set; }
	}
}