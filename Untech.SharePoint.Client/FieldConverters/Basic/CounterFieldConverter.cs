using System;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.FieldConverters.Basic
{
	[SpFieldConverter("Counter")]
	internal class CounterFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }
		public Type PropertyType { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			return (int)value;
		}

		public object ToSpValue(object value)
		{
			return (int)value;
		}

		public string ToCamlValue(object value)
		{
			return Convert.ToString(value);
		}
	}
}