using System.Collections.Generic;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.Data
{
	public interface ICommonService
	{
		IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors { get; } 
	}
}