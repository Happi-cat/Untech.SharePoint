using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.MetaModels.Collections;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaContentType : MetaModel
	{
		public MetaContentType(MetaList list, Type entityType, IReadOnlyCollection<IMetaFieldProvider> fieldProviders)
		{
			Guard.CheckNotNull("list", list);
			Guard.CheckNotNull("entityType", entityType);
			Guard.CheckNotNull("fieldProviders", fieldProviders);

			List = list;
			EntityType = entityType;

			Fields = new MetaFieldCollection(fieldProviders.Select(n => n.GetMetaField(this)));
		}

		public MetaList List { get; private set; }

		public Type EntityType { get; private set; }

		public MetaFieldCollection Fields { get; private set; }

		public string ContentTypeId { get; set; }

		public override void Accept(IMetaModelVisitor visitor)
		{
			visitor.VisitContentType(this);
		}
	}
}