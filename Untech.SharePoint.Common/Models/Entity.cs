using System;
using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Models
{
	[SpContentType(ContentTypeId = "0x01")]
	public class Entity
	{
		[SpField(InternalName = "ID", TypeAsString = "Counter")]
		public virtual int Id { get; set; }

		[SpField(TypeAsString = "Text")]
		public virtual string Title { get; set; }

		[SpField(TypeAsString = "DateTime")]
		public virtual DateTime Created { get; set; }

		[SpField(TypeAsString = "DateTime")]
		public virtual DateTime Modified { get; set; }

		[SpField(TypeAsString = "ContentTypeId")]
		public virtual string ContentTypeId { get; set; }
	}
}