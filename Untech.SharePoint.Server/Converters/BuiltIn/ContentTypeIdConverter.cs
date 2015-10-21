using Microsoft.SharePoint;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("ContentTypeId")]
	internal class ContentTypeIdConverter : IFieldConverter
	{
		public MetaField Field { get; set; }

		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			return value != null ? value.ToString() : null;
		}

		public object ToSpValue(object value)
		{
			return new SPContentTypeId((string) value);
		}

		public string ToCamlValue(object value)
		{
			return value != null ? value.ToString() : null;
		}
	}
}