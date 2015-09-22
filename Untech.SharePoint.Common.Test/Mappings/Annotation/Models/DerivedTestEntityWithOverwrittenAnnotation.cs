using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class DerivedTestEntityWithOverwrittenAnnotation : TestEntity
	{
		[SpField(Name = "HtmlBody")]
		public string Html { get; set; }

		[SpField(Name = "NewInternalName")]
		public override string OverrideProperty { get; set; }
	}
}