using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Geolocation")]
	internal class GeolocationFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			Guard.ThrowIfNot<SPFieldGeolocation>(Field, "This Field Converter doesn't support that SPField type");

			if (value == null)
				return null;

			return new GeoInfo(new SPFieldGeolocationValue(value.ToString()));
		}

		public object ToSpValue(object value)
		{
			Guard.ThrowIfNot<SPFieldGeolocation>(Field, "This Field Converter doesn't support that SPField type");

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