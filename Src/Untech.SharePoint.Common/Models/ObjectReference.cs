using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Untech.SharePoint.CodeAnnotations;

namespace Untech.SharePoint.Models
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

		/// <inheritdoc />
		public bool Equals(ObjectReference other)
		{
			return other != null && Id == other.Id;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((ObjectReference) obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return Id;
		}

		/// <inheritdoc />
		public static bool operator ==(ObjectReference left, ObjectReference right)
		{
			if (ReferenceEquals(left, right)) return true;
			if (ReferenceEquals(null, left) || ReferenceEquals(null, right)) return false;

			return left.Equals(right);
		}

		/// <inheritdoc />
		public static bool operator !=(ObjectReference left, ObjectReference right)
		{
			return !(left == right);
		}
	}
}
