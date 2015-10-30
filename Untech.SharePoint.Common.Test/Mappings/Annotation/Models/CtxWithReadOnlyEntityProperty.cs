using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class CtxWithReadOnlyEntityProperty : Ctx
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithReadOnlyProperty> InvalidEntity { get; set; }
	}
}