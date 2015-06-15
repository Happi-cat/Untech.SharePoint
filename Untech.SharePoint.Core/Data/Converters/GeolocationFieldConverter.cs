using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Geolocation")]
	internal class GeolocationFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldGeolocation>(field, "This Field Converter doesn't support that SPField type");

			if (value == null)
				return null;

			return new GeoInfo(new SPFieldGeolocationValue(value.ToString()));
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldGeolocation>(field, "This Field Converter doesn't support that SPField type");

			if (value == null)
				return null;

			if (!(value is GeoInfo))
			{
				throw new ArgumentException();
			}
			var geoInfo = (GeoInfo) value;

			return new SPFieldGeolocationValue(geoInfo.Latitude, geoInfo.Longitude, geoInfo.Altitude, geoInfo.Measure);
		}
	}
}