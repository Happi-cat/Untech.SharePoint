using System;
using Microsoft.SharePoint;
using Newtonsoft.Json;

namespace Untech.SharePoint.Core.Models
{
	[Serializable]
	public class GeoInfo
	{
		public GeoInfo()
		{

		}

		internal GeoInfo(SPFieldGeolocationValue geoValue)
		{
			Altitude = geoValue.Altitude;
			Latitude = geoValue.Latitude;
			Longitude = geoValue.Longitude;
			Measure = geoValue.Measure;
		}

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