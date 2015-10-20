using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.MetaModels.Collections;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaList : BaseMetaModel
	{
		public MetaList(MetaContext context, string listTitle, IReadOnlyCollection<IMetaContentTypeProvider> contentTypeProviders)
		{
			Guard.CheckNotNull("context", context);
			Guard.CheckNotNull("listTitle", listTitle);
			Guard.CheckNotNullOrEmpty("contentTypeProviders", contentTypeProviders);

			Context = context;
			Title = listTitle;
			
			ContentTypes = new MetaContentTypeCollection(contentTypeProviders.Select(n => n.GetMetaContentType(this)));
		}

		public string Title { get; private set; }

		public MetaContext Context { get; private set; }

		public MetaContentTypeCollection ContentTypes { get; private set; }

		public override void Accept(IMetaModelVisitor visitor)
		{
			visitor.VisitList(this);
		}
	}
}