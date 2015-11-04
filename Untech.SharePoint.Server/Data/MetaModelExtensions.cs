using JetBrains.Annotations;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Server.Data
{
	internal static class MetaModelExtensions
	{
		private const string SPWeb = "SPWeb";

		public static SPWeb GetSpWeb([NotNull]this MetaField contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			return contentType.GetAdditionalProperty<SPWeb>(SPWeb);
		}

		public static void SetSpWeb([NotNull]this MetaField field, [NotNull]SPWeb web)
		{
			Guard.CheckNotNull("field", field);
			Guard.CheckNotNull("web", web);

			field.SetAdditionalProperty(SPWeb, web);
		}
	}
}