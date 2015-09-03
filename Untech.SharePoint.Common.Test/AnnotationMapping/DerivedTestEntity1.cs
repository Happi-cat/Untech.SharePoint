using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	[SpContentType(ContentTypeId = "0x001020")]
	public class DerivedTestEntity1 : TestEntity
	{
		[SpField(InternalName = "HtmlBody")]
		public string Html { get; set; }

		public override string OverrideProperty { get; set; }
	}
}