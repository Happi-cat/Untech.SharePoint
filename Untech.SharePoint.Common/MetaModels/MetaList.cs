using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaList
	{
		public MetaList(MetaContext context, string listTitle, IReadOnlyCollection<IMetaContentTypeProvider> contentTypeProviders)
		{
			Guard.CheckNotNull("context", context);
			Guard.CheckNotNull("listTitle", listTitle);
			Guard.CheckNotNullOrEmpty("contentTypeProviders", contentTypeProviders);

			Context = context;
			ListTitle = listTitle;
			
			ContentTypes = contentTypeProviders
				.Select(n => n.GetMetaContentType(this))
				.ToList()
				.AsReadOnly();
		}

		public MetaContext Context { get; private set; }

		public string ListTitle { get; private set; }

		public IReadOnlyCollection<MetaContentType> ContentTypes { get; private set; }
	}
}