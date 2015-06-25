using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Models;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("Geolocation")]
	internal class GeolocationFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Guard.NotNull(field, "field");
			Guard.NotNull(propertyType, "propertyType");

			Guard.TypeIs<SPFieldGeolocationValue>(field.FieldValueType, "field.FieldValueType");

			Guard.TypeIs<GeoInfo>(propertyType, "propertType");

			Field = field;
			PropertyType = propertyType;
		}

		public object FromSpValue(object value)
		{
			if (value == null)
				return null;

			return new GeoInfo(new SPFieldGeolocationValue(value.ToString()));
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			var geoInfo = (GeoInfo) value;

			return new SPFieldGeolocationValue(geoInfo.Latitude, geoInfo.Longitude, geoInfo.Altitude, geoInfo.Measure);
		}
	}
}