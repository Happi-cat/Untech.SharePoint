using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	[SpContentType(ContentTypeId = "0x0010")]
	public class TestEntity
	{
		[SpField]
		public string Title { get; set; }

		[SpField(InternalName = "Details")]
		public string Description { get; set; }

		[SpField(InternalName = "OldInternalName")]
		public virtual string OverrideProperty { get; set; }

		public string MissingAttribute { get; set; }
	}
}