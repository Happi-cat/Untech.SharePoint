using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Models
{
	/// <summary>
	/// Represents lookup field value
	/// </summary>
	[PublicAPI]
	[DataContract]
	public class ObjectReference : IEquatable<ObjectReference>
	{
		/// <summary>
		/// Gets or sets lookup id.
		/// </summary>
		[DataMember]
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets lookup value.
		/// </summary>
		[DataMember]
		[JsonProperty("value")]
		public string Value { get; set; }

		/// <summary>
		/// Gets or sets list id.
		/// </summary>
		[DataMember]
		[JsonProperty("listId")]
		public Guid ListId { get; set; }

		public bool Equals(ObjectReference other)
		{
			return Id == other.Id;
		}

		public static bool operator ==(ObjectReference left, ObjectReference right)
		{
			if (ReferenceEquals(left, right)) return true;
			if (ReferenceEquals(null, left) || ReferenceEquals(null, right)) return false;

			return left.Equals(right);
		}

		public static bool operator !=(ObjectReference left, ObjectReference right)
		{
			return !(left == right);
		}
	}
}
