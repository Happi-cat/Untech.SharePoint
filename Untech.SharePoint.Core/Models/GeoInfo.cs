using System;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Models
{
	[Serializable]
	public class GeoInfo
	{
		[JsonProperty(PropertyName = "altitude", DefaultValueHandling = DefaultValueHandling.Populate)]
		public double Altitude { get; set; }

		[JsonProperty(PropertyName = "latitude", DefaultValueHandling = DefaultValueHandling.Populate)]
		public double Latitude { get; set; }

		[JsonProperty(PropertyName = "longitude", DefaultValueHandling = DefaultValueHandling.Populate)]
		public double Longitude { get; set; }

		[JsonProperty(PropertyName = "measure", DefaultValueHandling = DefaultValueHandling.Populate)]
		public double Measure { get; set; }
	}
}