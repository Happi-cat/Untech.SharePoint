using System;
using Microsoft.SharePoint;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Models
{
	[Serializable]
	public class UrlInfo
	{
		public UrlInfo()
		{
			
		}

		internal UrlInfo(SPFieldUrlValue value)
		{
			Url = value.Url;
			Description = value.Description;
		}

		[JsonProperty]
		public string Url { get; set; }

		[JsonProperty]
		public string Description { get; set; }
	}
}