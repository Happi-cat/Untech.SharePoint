using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Converters.BuiltIn
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
			return (string)value;
		}

		public string ToCamlValue(object value)
		{
			return (string)value;
		}
	}
}