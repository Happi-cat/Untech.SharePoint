using Untech.SharePoint.Common.Visitors;

namespace Untech.SharePoint.Common.MetaModels
{
	public interface IMetaModel
	{
		void Accept(IMetaModelVisitor visitor);
	}
}
