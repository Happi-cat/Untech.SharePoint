using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Guid")]
	internal class GuidFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldGuid>(field, "This Field Converter doesn't support that SPField type");

			return (Guid?)value;
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			Guard.ThrowIfNot<SPFieldGuid>(field, "This Field Converter doesn't support that SPField type");

			return value;
		}
	}
}