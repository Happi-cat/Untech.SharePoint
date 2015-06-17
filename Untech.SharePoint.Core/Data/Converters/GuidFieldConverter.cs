using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Guid")]
	internal class GuidFieldConverter : IFieldConverter
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
			Guard.ThrowIfNot<SPFieldGuid>(Field, "This Field Converter doesn't support that SPField type");

			return (Guid?)value;
		}

		public object ToSpValue(object value)
		{
			Guard.ThrowIfNot<SPFieldGuid>(Field, "This Field Converter doesn't support that SPField type");

			return value;
		}
	}
}