using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class ContextWithInvalidContextPropertyType : TestContext
	{
		[SpList(Title = "WrongPropertyType")]
		public bool WrongPropertyType { get; set; }
	}
}