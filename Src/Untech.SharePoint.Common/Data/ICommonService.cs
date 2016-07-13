using System.Collections.Generic;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.Data
{
	/// <summary>
	/// Represents interface of services that can be used inside <see cref="SpContext{TContext}"/>.
	/// </summary>
	public interface ICommonService
	{
		[NotNull]
		Config Config { get; }

		/// <summary>
		/// Gets ordered collection of <see cref="IMetaModel"/> processors.
		/// </summary>
		[NotNull]
		IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors { get; }

        [NotNull]
        ISpListItemsProvider GetItemsProvider([NotNull]MetaList list);
    }
}