using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Boolean")]
	public class BooleanFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			if (propertyType == null) throw new ArgumentNullException("propertyType");
			if (!(field is SPFieldBoolean)) 
				throw new ArgumentException("Converter doesn't support this SPField type", "field");

			if (propertyType.IsNullableType())
				return (bool?)value;

			return (bool?)value ?? false;
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			if (!(field is SPFieldBoolean)) 
				throw new ArgumentException("Converter doesn't support this SPField type", "field");

			return (bool?)value;
		}
	}
}