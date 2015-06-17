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

			if (field == null) throw new ArgumentNullException("field");
			if (propertyType == null) throw new ArgumentNullException("propertyType");

			if (field.FieldValueType != typeof(SPFieldGeolocationValue))
				throw new ArgumentException("SPField with SPFieldGeolocationValue value type only supported");

			if (propertyType != typeof(GeoInfo))
				throw new ArgumentException("This converter can be used only GeoInfo property types");

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