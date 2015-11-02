using System;
using Newtonsoft.Json;

namespace Untech.SharePoint.Common.Models
{
	/// <summary>
	/// Represents user info
	/// </summary>
	[Serializable]
	public class UserInfo
	{
		/// <summary>
		/// Gets or sets user id.
		/// </summary>
		[JsonProperty]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets user name.
		/// </summary>
		[JsonProperty]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets user login.
		/// </summary>
		[JsonProperty]
		public string Login { get; set; }

		/// <summary>
		/// Gets or sets user email.
		/// </summary>
		[JsonProperty]
		public string Email { get; set; }
	}
}
