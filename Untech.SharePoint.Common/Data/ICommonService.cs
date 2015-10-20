using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.Data
{
	public interface ICommonService
	{
		IMetaModelVisitor MetaModelProcessor { get; } 
	}
}