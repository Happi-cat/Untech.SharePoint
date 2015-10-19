using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.MetaModels.Collections;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaContext : IMetaModel
	{
		public MetaContext(IReadOnlyCollection<IMetaListProvider> listProviders)
		{
			Guard.CheckNotNull("listProviders", listProviders);

			Lists = new MetaListCollection(listProviders.Select(n => n.GetMetaList(this)));
		}

		public MetaListCollection Lists { get; private set; }

		public void Accept(IMetaModelVisitor visitor)
		{
			visitor.VisitContext(this);
		}
	}
}
