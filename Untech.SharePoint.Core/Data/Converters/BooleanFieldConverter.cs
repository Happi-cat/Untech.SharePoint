using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Boolean")]
	public class BooleanFieldConverter : IFieldConverter
	{
		public SPField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(SPField field, Type propertyType)
		{
			Field = field;
			PropertyType = propertyType;

			if (PropertyType == null) throw new ArgumentNullException("propertyType");
			if (!(Field is SPFieldBoolean))
				throw new ArgumentException("Converter doesn't support this SPField type", "field");
		}

		public object FromSpValue(object value)
		{
			if (PropertyType.IsNullableType())
				return (bool?)value;

			return (bool?)value ?? false;
		}

		public object ToSpValue(object value)
		{
			return (bool?)value;
		}
	}
}