using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaContext
	{
		public MetaContext(IReadOnlyCollection<IMetaListProvider> listProviders)
		{
			Guard.CheckNotNull("listProviders", listProviders);

			Lists = new ReadOnlyDictionary<string, MetaList>(listProviders
				.Select(n => n.GetMetaList(this))
				.ToDictionary(n => n.ListTitle));
		}

		public IReadOnlyDictionary<string, MetaList> Lists { get; private set; } 
	}
}
