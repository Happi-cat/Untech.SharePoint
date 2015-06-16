using System;
using Microsoft.SharePoint;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Models
{
	[Serializable]
	public class UserInfo
	{
		public UserInfo()
		{ }

		internal UserInfo(SPUser user)
		{
			Email = user.Email;
			Login = user.LoginName;
			Name = user.Name;
			Id = user.ID;
		}

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
