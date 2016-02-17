using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Models
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

		public bool Equals(UserInfo other)
		{
			return Id == other.Id;
		}

		public static bool operator ==(UserInfo left, UserInfo right)
		{
			if (ReferenceEquals(left, right)) return true;
			if (ReferenceEquals(null, left) || ReferenceEquals(null, right)) return false;

			return left.Equals(right);
		}

		public static bool operator !=(UserInfo left, UserInfo right)
		{
			return !(left == right);
		}
	}
}
