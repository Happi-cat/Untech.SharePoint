using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal class AnnotatedListMapping : IMetaListProvider
	{
		public AnnotatedListMapping(string listTitle)
		{
			Guard.CheckNotNull("listTitle", listTitle);

			ListTitle = listTitle;
			ContentTypeProviders = new Dictionary<Type, AnnotatedContentTypeMapping>();
		}

		public string ListTitle { get; private set; }

		public Dictionary<Type, AnnotatedContentTypeMapping> ContentTypeProviders { get; private set; }

		public AnnotatedContentTypeMapping GetOrAddContentType(Type entityType, Func<AnnotatedContentTypeMapping> builder)
		{
			if (!ContentTypeProviders.ContainsKey(entityType))
			{
				ContentTypeProviders.Add(entityType, builder());
			}

			return ContentTypeProviders[entityType];
		}

		public MetaList GetMetaList(MetaContext parent)
		{
			return new MetaList(parent, ListTitle, ContentTypeProviders.Values.ToList());
		}
	}
}