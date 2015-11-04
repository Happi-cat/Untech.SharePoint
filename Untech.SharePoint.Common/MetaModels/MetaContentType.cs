using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Untech.SharePoint.Common.MetaModels.Collections;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	/// <summary>
	/// Represents MetaData for SP ContentType
	/// </summary>
	public sealed class MetaContentType : BaseMetaModel
	{
		/// <summary>
		/// Initializes new instance of <see cref="MetaContentType"/>.
		/// </summary>
		/// <param name="list">Metadata of parent SP List.</param>
		/// <param name="entityType">Equivalent .NET type.</param>
		/// <param name="fieldProviders">Providers of <see cref="MetaField"/> that associated with current content type.</param>
		/// <exception cref="ArgumentNullException"><paramref name="list"/> or <paramref name="entityType"/> or <paramref name="fieldProviders"/> are null.</exception>
		public MetaContentType([NotNull]MetaList list, [NotNull]Type entityType, [NotNull]IReadOnlyCollection<IMetaFieldProvider> fieldProviders)
		{
			Guard.CheckNotNull("list", list);
			Guard.CheckNotNull("entityType", entityType);
			Guard.CheckNotNull("fieldProviders", fieldProviders);

			List = list;
			EntityType = entityType;

			Fields = new MetaFieldCollection(fieldProviders.Select(n => n.GetMetaField(this)));
		}

		/// <summary>
		/// Gets or sets ConetnTypeId
		/// </summary>
		[CanBeNull]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets ContentType display name.
		/// </summary>
		[CanBeNull]
		public string Name { get; set; }

		/// <summary>
		/// Gets parent <see cref="MetaList"/>
		/// </summary>
		[NotNull]
		public MetaList List { get; private set; }

		/// <summary>
		/// Gets collection of child <see cref="MetaField"/>
		/// </summary>
		[NotNull]
		public MetaFieldCollection Fields { get; private set; }

		/// <summary>
		/// Gets <see cref="Type"/> of associated entity.
		/// </summary>
		[NotNull]
		public Type EntityType { get; private set; }

		/// <summary>
		/// Accepts <see cref="IMetaModelVisitor"/> instance.
		/// </summary>
		/// <param name="visitor">Visitor to accept.</param>
		public override void Accept([NotNull]IMetaModelVisitor visitor)
		{
			visitor.VisitContentType(this);
		}
	}
}