using System;
using Microsoft.SharePoint;

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


		public double Altitude { get; set; }
		
		public double Latitude { get; set; }
		
		public double Longitude { get; set; }
	
		public double Measure { get; set; }
	}
}