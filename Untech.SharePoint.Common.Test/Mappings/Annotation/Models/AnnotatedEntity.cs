using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	[SpContentType(Id = "0x0010")]
	public class AnnotatedEntity
	{
		[SpField]
		public string Field = "Test";

		[SpField]
		public string Title { get; set; }

		[SpField(Name = "Details")]
		public string Description { get; set; }

		[SpField(Name = "OldInternalName")]
		public virtual string OverrideProperty { get; set; }

		[SpField]
		[SpFieldRemoved]
		public virtual string RemovedSpField { get; set; }

		public string MissingAttribute { get; set; }
	}
}