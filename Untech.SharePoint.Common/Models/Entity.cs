using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Models
{
	/// <summary>
	/// Represents base SP content type entity.
	/// </summary>
	[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	[SpContentType(Id = "0x01")]
	[DataContract]
	[SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
	public class Entity
	{
		/// <summary>
		/// Gets or sets item ID.
		/// </summary>
		[SpField(Name = "ID", FieldType = "Counter")]
		[DataMember]
		public virtual int Id { get; set; }

		/// <summary>
		/// Gets or sets item title.
		/// </summary>
		[SpField(FieldType = "Text")]
		[DataMember]
		public virtual string Title { get; set; }

		/// <summary>
		/// Gets or sets item creation date.
		/// </summary>
		[SpField(FieldType = "DateTime")]
		[DataMember]
		public virtual DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets item modification date.
		/// </summary>
		[SpField(FieldType = "DateTime")]
		[DataMember]
		public virtual DateTime Modified { get; set; }

		/// <summary>
		/// Gets or sets item content type id.
		/// </summary>
		[SpField(FieldType = "ContentTypeId")]
		[DataMember]
		public virtual string ContentTypeId { get; set; }
	}
}