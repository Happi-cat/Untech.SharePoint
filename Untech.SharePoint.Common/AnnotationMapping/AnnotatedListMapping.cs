using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal class AnnotatedListMapping : IMetaListProvider
	{
		private readonly string _title;
		private readonly Dictionary<Type, AnnotatedContentTypeMapping> _contentTypeProviders;

		public AnnotatedListMapping(string listTitle)
		{
			Guard.CheckNotNull("listTitle", listTitle);

			_title = listTitle;
			_contentTypeProviders = new Dictionary<Type, AnnotatedContentTypeMapping>();
		}

		public AnnotatedContentTypeMapping GetOrAddContentType(Type entityType, Func<AnnotatedContentTypeMapping> builder)
		{
			if (!_contentTypeProviders.ContainsKey(entityType))
			{
				_contentTypeProviders.Add(entityType, builder());
			}

			return _contentTypeProviders[entityType];
		}

		public MetaList GetMetaList(MetaContext parent)
		{
			return new MetaList(parent, _title, _contentTypeProviders.Values.ToList());
		}
	}
}