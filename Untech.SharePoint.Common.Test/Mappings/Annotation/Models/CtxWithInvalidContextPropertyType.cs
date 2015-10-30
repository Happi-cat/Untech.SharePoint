using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class CtxWithInvalidContextPropertyType : Ctx
	{
		[SpList(Title = "WrongPropertyType")]
		public bool WrongPropertyType { get; set; }
	}
}