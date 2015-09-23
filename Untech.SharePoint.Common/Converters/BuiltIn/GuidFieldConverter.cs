using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters.BuiltIn
{
	[SpFieldConverter("Guid")]
	internal class GuidFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			return (Guid?)value ?? Guid.Empty;
		}

		public object ToSpValue(object value)
		{
			if (value == null)
				return null;

			var guidValue = (Guid) value;
			if (guidValue == Guid.Empty)
				return null;
			
			return guidValue;
		}

		public string ToCamlValue(object value)
		{
			return Convert.ToString(ToSpValue(value));
		}
	}
}