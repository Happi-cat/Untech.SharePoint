using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data;
using Untech.SharePoint.MetaModels.Collections;
using Untech.SharePoint.MetaModels.Providers;
using Untech.SharePoint.MetaModels.Visitors;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.MetaModels
{
	/// <summary>
	/// Represents MetaData of <see cref="ISpContext"/>
	/// </summary>
	public sealed class MetaContext : BaseMetaModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MetaContext"/>.
		/// </summary>
		/// <param name="listProviders">Providers of <see cref="MetaList"/> associated with the current context.</param>
		/// <exception cref="ArgumentNullException"><paramref name="listProviders"/> is null.</exception>
		public MetaContext([NotNull]IReadOnlyCollection<IMetaListProvider> listProviders)
		{
			Guard.CheckNotNull(nameof(listProviders), listProviders);

			Lists = new MetaListCollection(listProviders.Select(n => n.GetMetaList(this)));
		}

		/// <summary>
		/// Gets collection of child <see cref="MetaList"/>.
		/// </summary>
		[NotNull]
		public MetaListCollection Lists { get; }

		/// <summary>
		/// Gets or sets SP Web URL.
		/// </summary>
		[CanBeNull]
		public string Url { get; set; }

		/// <summary>
		/// Accepts <see cref="IMetaModelVisitor"/> instance.
		/// </summary>
		/// <param name="visitor">Visitor to accept.</param>
		public override void Accept([NotNull]IMetaModelVisitor visitor)
		{
			visitor.VisitContext(this);
		}
	}
}
