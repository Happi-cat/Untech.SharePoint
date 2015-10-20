using System;
using Newtonsoft.Json;

namespace Untech.SharePoint.Common.Models
{
	[Serializable]
	public class UserInfo
	{
		[JsonProperty]
		public int Id { get; set; }

		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public string Login { get; set; }

		[JsonProperty]
		public string Email { get; set; }
	}
}
