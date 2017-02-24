using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Data
{
	internal static class MetaModelExtensions
	{
		private const string SPWeb = "SPWeb";

		public static SPWeb GetSpWeb([NotNull]this MetaField contentType)
		{
			Guard.CheckNotNull(nameof(contentType), contentType);

			return contentType.GetAdditionalProperty<SPWeb>(SPWeb);
		}

		public static void SetSpWeb([NotNull]this MetaField field, [NotNull]SPWeb web)
		{
			Guard.CheckNotNull(nameof(field), field);
			Guard.CheckNotNull(nameof(web), web);

			field.SetAdditionalProperty(SPWeb, web);
		}
	}
}