using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Collections;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	/// <summary>
	/// Represents MetaData for SP List
	/// </summary>
	[PublicAPI]
	public sealed class MetaList : BaseMetaModel
	{
		/// <summary>
		/// Initializes new instance of <see cref="MetaList"/>
		/// </summary>
		/// <param name="context">Metadata of parent <see cref="ISpContext"/></param>
		/// <param name="listUrl">Current list url</param>
		/// <param name="contentTypeProviders">Providers of <see cref="MetaContentType"/> that associated with current list.</param>
		/// <exception cref="ArgumentNullException">any parameter is null.</exception>
		public MetaList([NotNull]MetaContext context, [NotNull]string listUrl, [NotNull]IReadOnlyCollection<IMetaContentTypeProvider> contentTypeProviders)
		{
			Guard.CheckNotNull(nameof(context), context);
			Guard.CheckNotNull(nameof(listUrl), listUrl);
			Guard.CheckNotNullOrEmpty(nameof(contentTypeProviders), contentTypeProviders);

			Context = context;
			Url = listUrl;

			ContentTypes = new MetaContentTypeCollection(contentTypeProviders.Select(n => n.GetMetaContentType(this)));
		}

		/// <summary>
		/// Gets the site-relative URL at which the list was placed. 
		/// </summary>
		[NotNull]
		public string Url { get; private set; }

		/// <summary>
		/// Gets or sets current list title.
		/// </summary>
		[CanBeNull]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets whether this list is external.
		/// </summary>
		public bool IsExternal { get; set; }

		/// <summary>
		/// Gets parent <see cref="MetaContext"/>.
		/// </summary>
		[NotNull]
		public MetaContext Context { get; }

		/// <summary>
		/// Gets collection of child <see cref="MetaContentType"/>.
		/// </summary>
		[NotNull]
		public MetaContentTypeCollection ContentTypes { get; }

		/// <summary>
		/// Accepts <see cref="IMetaModelVisitor"/> instance.
		/// </summary>
		/// <param name="visitor">Visitor to accept.</param>
		public override void Accept([NotNull]IMetaModelVisitor visitor)
		{
			visitor.VisitList(this);
		}
	}
}