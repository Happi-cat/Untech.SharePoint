using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class CtxWithWriteOnlyEntityProperty : Ctx
	{
		[SpList(Title = "Test")]
		public ISpList<EntityWithWriteOnlyProperty> InvalidEntity { get; set; }
	}
}