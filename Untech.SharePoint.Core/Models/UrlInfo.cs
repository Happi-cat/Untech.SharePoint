using System;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Models
{
	[Serializable]
	public class UrlInfo
	{
		[JsonProperty]
		public string Url { get; set; }

		[JsonProperty]
		public string Description { get; set; }
	}
}