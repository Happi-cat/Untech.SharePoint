using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters.BuiltIn
{
	[SPFieldConverter("Counter")]
	internal class CounterFieldConverter : IFieldConverter
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
			return (int)value;
		}

		public object ToSpValue(object value)
		{
			return value;
		}
	}
}