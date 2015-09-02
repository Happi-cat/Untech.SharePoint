using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaContentType
	{
		public MetaContentType(MetaList list, Type modelType, IReadOnlyCollection<IMetaFieldProvider> fieldProviders)
		{
			Guard.CheckNotNull("list", list);
			Guard.CheckNotNull("modelType", modelType);
			Guard.CheckNotNull("fieldProviders", fieldProviders);

			List = list;
			ModelType = modelType;

			Fields = new ReadOnlyDictionary<string, MetaField>(fieldProviders
				.Select(n => n.GetMetaField(this))
				.ToDictionary(n => n.MemberName));
		}

		public MetaList List { get; private set; }

		public Type ModelType { get; private set; }

		public IReadOnlyDictionary<string, MetaField> Fields { get; private set; }

		public string ContentTypeId { get; set; }
	}
}