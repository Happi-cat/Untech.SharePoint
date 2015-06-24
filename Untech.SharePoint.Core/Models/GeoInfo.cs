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

		[JsonProperty]
		public double Altitude { get; set; }

		[JsonProperty]
		public double Latitude { get; set; }

		[JsonProperty]
		public double Longitude { get; set; }

		[JsonProperty]
		public double Measure { get; set; }
	}
}