using Untech.SharePoint.Common.Visitors;

namespace Untech.SharePoint.Common.MetaModels
{
	public abstract class MetaModel
	{
		public virtual void Accept(IMetaModelVisitor visitor)
		{
			visitor.VisitUnkown(this);
		}
	}
}
