using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Server.MetaModels.Visitors
{
	internal class ContentTypeCreatorInitializer : BaseMetaModelVisitor
	{
		public override void VisitContentType(MetaContentType contentType)
		{
			contentType.EntityTypeCreator = InstanceCreationUtility.GetCreator<object>(contentType.EntityType);
		}
	}
}