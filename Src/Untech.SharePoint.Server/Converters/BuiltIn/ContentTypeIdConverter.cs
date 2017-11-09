using Microsoft.SharePoint;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Converters;
using Untech.SharePoint.MetaModels;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Server.Converters.BuiltIn
{
	[SpFieldConverter("ContentTypeId")]
	[UsedImplicitly]
	internal class ContentTypeIdConverter : IFieldConverter
	{
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull(nameof(field), field);
		}

		public object FromSpValue(object value)
		{
			return value?.ToString();
		}

		public object ToSpValue(object value)
		{
			return new SPContentTypeId((string)value);
		}

		public string ToCamlValue(object value)
		{
			return value?.ToString();
		}
	}
}