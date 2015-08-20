using System;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data.FieldConverters.Basic
{
	[SpFieldConverter("Counter")]
	internal class CounterFieldConverter : IFieldConverter
	{
		public Field Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(Field field, Type propertyType)
		{
			Guard.CheckNotNull("field", field);
			Guard.CheckNotNull("propertyType", propertyType);
			Guard.CheckType<int>("propertyType", propertyType);

			Field = field;
			PropertyType = propertyType;
		}

		public object FromClientValue(object value)
		{
			return (int)value;
		}

		public object ToClientValue(object value)
		{
			return (int)value;
		}

		public string ToCamlValue(object value)
		{
			return Convert.ToString(value);
		}
	}
}