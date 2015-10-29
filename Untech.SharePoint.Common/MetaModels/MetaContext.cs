using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Collections;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	/// <summary>
	/// Represents MetaData of <see cref="ISpContext"/>
	/// </summary>
	public sealed class MetaContext : BaseMetaModel
	{
		public MetaContext([NotNull]IReadOnlyCollection<IMetaListProvider> listProviders)
		{
			Guard.CheckNotNull("listProviders", listProviders);

			Lists = new MetaListCollection(listProviders.Select(n => n.GetMetaList(this)));
		}

		[NotNull]
		public MetaListCollection Lists { get; private set; }

		/// <summary>
		/// Accepts <see cref="IMetaModelVisitor"/>.
		/// </summary>
		/// <param name="visitor">Visitor to accept.</param>
		public override void Accept([NotNull]IMetaModelVisitor visitor)
		{
			visitor.VisitContext(this);
		}
	}
}
