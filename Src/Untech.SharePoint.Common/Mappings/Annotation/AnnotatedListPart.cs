using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	internal class AnnotatedListPart : IMetaListProvider
	{
		private readonly string _url;
		private readonly Dictionary<Type, AnnotatedContentTypeMapping> _contentTypeProviders;

		private AnnotatedListPart(string listUrl)
		{
			Guard.CheckNotNullOrEmpty(nameof(listUrl), listUrl);

			_url = listUrl;
			_contentTypeProviders = new Dictionary<Type, AnnotatedContentTypeMapping>();
		}

		#region [Public Static]

		public static bool IsAnnotated(PropertyInfo property)
		{
			return property.IsDefined(typeof(SpListAttribute));
		}

		public static AnnotatedListPart Create(string listUrl, IEnumerable<PropertyInfo> contextProperties)
		{
			var listMapping = new AnnotatedListPart(listUrl);

			contextProperties.Each(listMapping.RegisterContentType);

			return listMapping;
		}

		#endregion

		public MetaList GetMetaList(MetaContext parent)
		{
			return new MetaList(parent, _url, _contentTypeProviders.Values.ToList());
		}

		#region [Private Methods]

		private void RegisterContentType(PropertyInfo contextProperty)
		{
			Rules.CheckContextList(contextProperty);

			var entityType = contextProperty.PropertyType.GetGenericArguments()[0];

			RegisterContentType(entityType);
		}

		private void RegisterContentType(Type entityType)
		{
			if (!_contentTypeProviders.ContainsKey(entityType))
			{
				_contentTypeProviders.Add(entityType, new AnnotatedContentTypeMapping(entityType));
			}
		}

		#endregion
	}
}