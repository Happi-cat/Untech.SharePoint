using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Untech.SharePoint.CodeAnnotations;

namespace Untech.SharePoint.Models
{
	/// <summary>
	/// Represents user info
	/// </summary>
	[PublicAPI]
	[DataContract]
	public class UserInfo : IEquatable<UserInfo>
	{
		/// <summary>
		/// Gets or sets user id.
		/// </summary>
		[DataMember]
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets user login.
		/// </summary>
		[DataMember]
		[JsonProperty("login")]
		public string Login { get; set; }

		/// <summary>
		/// Gets or sets user email.
		/// </summary>
		[DataMember]
		[JsonProperty("email")]
		public string Email { get; set; }

		/// <inheritdoc />
		public bool Equals(UserInfo other)
		{
			return other != null && Id == other.Id;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((UserInfo) obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return Id;
		}

		/// <inheritdoc />
		public static bool operator ==(UserInfo left, UserInfo right)
		{
			if (ReferenceEquals(left, right)) return true;
			if (ReferenceEquals(null, left) || ReferenceEquals(null, right)) return false;

			return left.Equals(right);
		}

		/// <inheritdoc />
		public static bool operator !=(UserInfo left, UserInfo right)
		{
			return !(left == right);
		}
	}
}
