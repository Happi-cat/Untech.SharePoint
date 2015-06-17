using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Number")]
	internal class NumberFieldConverter : IFieldConverter
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
			Guard.ThrowIfNot<SPFieldNumber>(Field, "This Field Converter doesn't support that SPField type");

			if (PropertyType.IsNullableType())
				return (double?)value;

			return (double?) value ?? 0;
		}

		public object ToSpValue(object value)
		{
			Guard.ThrowIfNot<SPFieldNumber>(Field, "This Field Converter doesn't support that SPField type");

			return (double?)value;
		}
	}
}
