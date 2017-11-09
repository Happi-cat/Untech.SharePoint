using System.Runtime.Serialization;
using Newtonsoft.Json;
using Untech.SharePoint.CodeAnnotations;

namespace Untech.SharePoint.Models
{
	/// <summary>
	/// Represents Geo info data.
	/// </summary>
	[PublicAPI]
	[DataContract]
	public class GeoInfo
	{
		/// <summary>
		/// Gets or sets altitude.
		/// </summary>
		[DataMember]
		[JsonProperty("altitude")]
		public double Altitude { get; set; }

		/// <summary>
		/// Gets or sets latitude.
		/// </summary>
		[DataMember]
		[JsonProperty("latitude")]
		public double Latitude { get; set; }

		/// <summary>
		/// Gets or sets longitude.
		/// </summary>
		[DataMember]
		[JsonProperty("longitude")]
		public double Longitude { get; set; }

		/// <summary>
		/// Gets or sets measure.
		/// </summary>
		[DataMember]
		[JsonProperty("measure")]
		public double Measure { get; set; }
	}
}