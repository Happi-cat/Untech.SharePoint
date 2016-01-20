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
	public class UserInfo
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
	}
}
