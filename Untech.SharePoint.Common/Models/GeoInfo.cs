using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Untech.SharePoint.Common.Models
{
	/// <summary>
	/// Represents geo info data.
	/// </summary>
	[Serializable]
	[DataContract]
	public class GeoInfo
	{
		/// <summary>
		/// Gets or sets altitude.
		/// </summary>
		[DataMember]
		[JsonProperty(PropertyName = "altitude", DefaultValueHandling = DefaultValueHandling.Populate)]
		public double Altitude { get; set; }

		/// <summary>
		/// Gets or sets latitude.
		/// </summary>
		[DataMember]
		[JsonProperty(PropertyName = "latitude", DefaultValueHandling = DefaultValueHandling.Populate)]
		public double Latitude { get; set; }

		/// <summary>
		/// Gets or sets longitude.
		/// </summary>
		[DataMember]
		[JsonProperty(PropertyName = "longitude", DefaultValueHandling = DefaultValueHandling.Populate)]
		public double Longitude { get; set; }

		/// <summary>
		/// Gets or sets measure.
		/// </summary>
		[DataMember]
		[JsonProperty(PropertyName = "measure", DefaultValueHandling = DefaultValueHandling.Populate)]
		public double Measure { get; set; }
	}
}