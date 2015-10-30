using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	[SpContentType(Id = "0x001020")]
	public class DerivedEntityWithIheritedAnnotation : Entity
	{
		[SpField(Name = "HtmlBody")]
		public string Html { get; set; }

		public override string OverrideProperty { get; set; }
	}
}