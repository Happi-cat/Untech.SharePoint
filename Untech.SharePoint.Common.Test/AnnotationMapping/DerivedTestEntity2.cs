using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	public class DerivedTestEntity2 : TestEntity
	{
		[SpField(InternalName = "HtmlBody")]
		public string Html { get; set; }

		[SpField(InternalName = "NewInternalName")]
		public override string OverrideProperty { get; set; }
	}
}