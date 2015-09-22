using System;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Models
{
	[SpContentType(Id = "0x01")]
	public class Entity
	{
		[SpField(Name = "ID", FieldType = "Counter")]
		public virtual int Id { get; set; }

		[SpField(FieldType = "Text")]
		public virtual string Title { get; set; }

		[SpField(FieldType = "DateTime")]
		public virtual DateTime Created { get; set; }

		[SpField(FieldType = "DateTime")]
		public virtual DateTime Modified { get; set; }

		[SpField(FieldType = "ContentTypeId")]
		public virtual string ContentTypeId { get; set; }
	}
}