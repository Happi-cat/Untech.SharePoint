using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Models
{
	/// <summary>
	/// Represents base SP content type entity.
	/// </summary>
	[PublicAPI]
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
		[JsonProperty("id")]
		public virtual int Id { get; set; }

		/// <summary>
		/// Gets or sets item title.
		/// </summary>
		[SpField(FieldType = "Text")]
		[DataMember]
		[JsonProperty("title")]
		public virtual string Title { get; set; }

		/// <summary>
		/// Gets or sets item creation date.
		/// </summary>
		[SpField(FieldType = "DateTime")]
		[DataMember]
		[JsonProperty("created")]
		public virtual DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets item author.
		/// </summary>
		[SpField(FieldType = "User")]
		[DataMember]
		[JsonProperty("author")]
		public virtual UserInfo Author { get; set; }

		/// <summary>
		/// Gets or sets item modification date.
		/// </summary>
		[SpField(FieldType = "DateTime")]
		[DataMember]
		[JsonProperty("modified")]
		public virtual DateTime Modified { get; set; }

		/// <summary>
		/// Gets or sets item editor.
		/// </summary>
		[SpField(FieldType = "User")]
		[DataMember]
		[JsonProperty("editor")]
		public virtual UserInfo Editor { get; set; }

		/// <summary>
		/// Gets or sets item content type id.
		/// </summary>
		[SpField(FieldType = "ContentTypeId")]
		[DataMember]
		[JsonProperty("contentTypeId")]
		public virtual string ContentTypeId { get; set; }
	}
}