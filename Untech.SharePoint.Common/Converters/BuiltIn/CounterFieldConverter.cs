using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Counter")]
	internal class CounterFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Field = field;
		}

		public object FromSpValue(object value)
		{
			return (int)value;
		}

		public object ToSpValue(object value)
		{
			return value;
		}

		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}
	}
}