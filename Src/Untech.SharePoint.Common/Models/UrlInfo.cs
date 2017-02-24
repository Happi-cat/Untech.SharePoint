using System.Runtime.Serialization;
using Newtonsoft.Json;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Models
{
	/// <summary>
	/// Represents URL info.
	/// </summary>
	[PublicAPI]
	[DataContract]
	public class UrlInfo
	{
		/// <summary>
		/// Gets or sets URL.
		/// </summary>
		[DataMember]
		[JsonProperty("URL")]
		public string Url { get; set; }

		/// <summary>
		/// Gets or sets URL description.
		/// </summary>
		[DataMember]
		[JsonProperty("description")]
		public string Description { get; set; }
	}
}