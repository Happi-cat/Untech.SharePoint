using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	[SpContentType(Id = "0x001020")]
	public class DerivedTestEntityWithIheritedAnnotation : TestEntity
	{
		[SpField(Name = "HtmlBody")]
		public string Html { get; set; }

		public override string OverrideProperty { get; set; }
	}
}