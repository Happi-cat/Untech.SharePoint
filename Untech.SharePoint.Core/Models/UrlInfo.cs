using System;
using System.Runtime.Serialization;
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
			Title = value.Description;
		}

		[JsonProperty]
		public string Url { get; set; }

		[JsonProperty]
		public string Title { get; set; }
	}
}