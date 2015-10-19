using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.MetaModels
{
	public interface IMetaModel
	{
		void Accept(IMetaModelVisitor visitor);
	}
}
