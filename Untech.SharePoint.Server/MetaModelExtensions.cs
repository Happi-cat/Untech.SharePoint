using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server
{
	internal static class MetaModelExtensions
	{
		public static Materiliazer Materiliazer(this MetaContentType contentType)
		{
			return contentType.GetAdditionalProperty<Materiliazer>("Materializer");
		}
	}
}