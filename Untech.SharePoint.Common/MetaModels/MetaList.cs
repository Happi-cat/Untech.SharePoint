using System;
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
	/// Represents MetaData for SP List
	/// </summary>
	public sealed class MetaList : BaseMetaModel
	{
		/// <summary>
		/// Initializes new instance of <see cref="MetaList"/>
		/// </summary>
		/// <param name="context">Metadata of parent <see cref="ISpContext"/></param>
		/// <param name="listTitle">Current list title</param>
		/// <param name="contentTypeProviders">Providers of <see cref="MetaContentType"/> that associated with current list.</param>
		/// <exception cref="ArgumentNullException">any parameter is null.</exception>
		public MetaList([NotNull]MetaContext context, [NotNull]string listTitle, [NotNull]IReadOnlyCollection<IMetaContentTypeProvider> contentTypeProviders)
		{
			Guard.CheckNotNull("context", context);
			Guard.CheckNotNull("listTitle", listTitle);
			Guard.CheckNotNullOrEmpty("contentTypeProviders", contentTypeProviders);

			Context = context;
			Title = listTitle;
			
			ContentTypes = new MetaContentTypeCollection(contentTypeProviders.Select(n => n.GetMetaContentType(this)));
		}

		/// <summary>
		/// Gets current list title.
		/// </summary>
		[NotNull]
		public string Title { get; private set; }

		/// <summary>
		/// Gets or sets whether this list is external.
		/// </summary>
		public bool IsExternal { get; set; }

		/// <summary>
		/// Gets parent <see cref="MetaContext"/>.
		/// </summary>
		[NotNull]
		public MetaContext Context { get; private set; }

		/// <summary>
		/// Gets collection of child <see cref="MetaContentType"/>.
		/// </summary>
		[NotNull]
		public MetaContentTypeCollection ContentTypes { get; private set; }

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