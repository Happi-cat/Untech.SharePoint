using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	[SpContentType(Id = "0x0010")]
	public class TestEntity
	{
		[SpField]
		public string Title { get; set; }

		[SpField(Name = "Details")]
		public string Description { get; set; }

		[SpField(Name = "OldInternalName")]
		public virtual string OverrideProperty { get; set; }

		[SpField]
		[SpFieldRemoved]
		public virtual string RemovedFromBeginning { get; set; }

		public string MissingAttribute { get; set; }
	}
}