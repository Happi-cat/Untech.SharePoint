using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("ContentTypeId")]
	[UsedImplicitly]
	internal class ContentTypeIdConverter : IFieldConverter
	{
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);
		}

		public object FromSpValue(object value)
		{
			return value?.ToString();
		}

		public object ToSpValue(object value)
		{
			return new SPContentTypeId((string) value);
		}

		public string ToCamlValue(object value)
		{
			return value?.ToString();
		}
	}
}