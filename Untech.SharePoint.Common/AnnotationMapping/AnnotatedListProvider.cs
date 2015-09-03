using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal class AnnotatedListProvider : IMetaListProvider
	{
		public AnnotatedListProvider(string listTitle)
		{
			ListTitle = listTitle;
			ContentTypeProviders = new Dictionary<Type, AnnotatedContentTypeProvider>();
		}

		public string ListTitle { get; private set; }

		public Dictionary<Type, AnnotatedContentTypeProvider> ContentTypeProviders { get; private set; }

		public AnnotatedContentTypeProvider GetOrAddContentType(Type entityType, Func<AnnotatedContentTypeProvider> builder)
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