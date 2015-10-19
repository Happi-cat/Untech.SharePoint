using Newtonsoft.Json;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.Custom
{
	public class JsonFieldConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			return string.IsNullOrEmpty((string) value) ? null : JsonConvert.DeserializeObject((string)value, Field.MemberType);
		}

		public object ToSpValue(object value)
		{
			return value == null ? null : JsonConvert.SerializeObject(value);
		}

		public string ToCamlValue(object value)
		{
			return (string) ToSpValue(value);
		}
	}
}