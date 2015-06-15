using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	[SPFieldConverter("Counter")]
	internal class CounterFieldConverter : IFieldConverter
	{
		public object FromSpValue(object value, SPField field, Type propertyType)
		{
			return (int)value;
		}

		public object ToSpValue(object value, SPField field, Type propertyType)
		{
			return value;
		}
	}
}