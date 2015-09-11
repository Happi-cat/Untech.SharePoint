using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	public class DerivedTestEntity2 : TestEntity
	{
		[SpField(Name = "HtmlBody")]
		public string Html { get; set; }

		[SpField(Name = "NewInternalName")]
		public override string OverrideProperty { get; set; }
	}
}