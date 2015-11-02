using System.Collections.Generic;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents interface of services that can be used inside <see cref="SpContext{TContext,TCommonService}"/>.
	/// </summary>
	public interface ICommonService
	{
		/// <summary>
		/// Gets ordered collection of <see cref="IMetaModel"/> processors.
		/// </summary>
		IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors { get; } 
	}
}