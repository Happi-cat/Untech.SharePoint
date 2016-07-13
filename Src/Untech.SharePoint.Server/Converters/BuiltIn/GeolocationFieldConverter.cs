using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("Geolocation")]
	[UsedImplicitly]
	internal class GeolocationFieldConverter : IFieldConverter
	{
		/// <summary>
		/// Initialzes current instance with the specified <see cref="MetaField"/>
		/// </summary>
		/// <param name="field"></param>
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			if (field.MemberType != typeof (GeoInfo))
			{
				throw new ArgumentException("Only GeoInfo member type is allowed.");
			}
		}

		/// <summary>
		/// Converts SP field value to <see cref="MetaField.MemberType"/>.
		/// </summary>
		/// <param name="value">SP value to convert.</param>
		/// <returns>Member value.</returns>
		public object FromSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}

			var spValue = (SPFieldGeolocationValue) value;

			return new GeoInfo
			{
				Altitude = spValue.Altitude,
				Latitude = spValue.Latitude,
				Longitude = spValue.Longitude,
				Measure = spValue.Measure
			};
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP field value.
		/// </summary>
		/// <param name="value">Member value to convert.</param>
		/// <returns>SP field value.</returns>
		public object ToSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}

			var geoInfo = (GeoInfo) value;

			return new SPFieldGeolocationValue(geoInfo.Latitude, geoInfo.Longitude, geoInfo.Altitude, geoInfo.Measure);
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP Caml value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Caml value.</returns>
		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}
	}
}