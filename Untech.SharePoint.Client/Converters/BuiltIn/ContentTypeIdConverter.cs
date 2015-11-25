using System;
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
			Guard.CheckNotNull("field", field);
		}

		public object FromSpValue(object value)
		{
			return value != null ? value.ToString() : null;
		}

		public object ToSpValue(object value)
		{
			throw new NotImplementedException();
		}

		public string ToCamlValue(object value)
		{
			return value != null ? value.ToString() : null;
		}
	}
}