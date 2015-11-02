using System;
using Newtonsoft.Json;

namespace Untech.SharePoint.Common.Models
{
	/// <summary>
	/// Represents url info.
	/// </summary>
	[Serializable]
	public class UrlInfo
	{
		/// <summary>
		/// Gets or sets url.
		/// </summary>
		[JsonProperty]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets url description.
		/// </summary>
		[JsonProperty]
		public string Description { get; set; }
	}
}