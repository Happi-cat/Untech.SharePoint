using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedContextMapping<T> : IMetaContextProvider
	{
		private readonly Type _contextType;
		private readonly Dictionary<string, AnnotatedListMapping> _listProviders;

		public AnnotatedContextMapping()
		{
			_contextType = typeof(T);
			_listProviders = new Dictionary<string, AnnotatedListMapping>();

			Initialize();
		}

		public MetaContext GetMetaContext()
		{
			return new MetaContext(_listProviders.Values.ToList());
		}

		public AnnotatedListMapping GetOrAddList(string listTitle)
		{
			if (!_listProviders.ContainsKey(listTitle))
			{
				_listProviders.Add(listTitle, new AnnotatedListMapping(listTitle));
			}

			return _listProviders[listTitle];
		}

		private void Initialize()
		{
			_contextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(AnnotationConventions.HasListAnnotatinon)
				.Each(RegisterList);
		}

		private void RegisterList(PropertyInfo property)
		{
			AnnotationConventions.ValidateList(property);

			var listProvider = GetOrAddList(ResolveListTitle(property));

			var entityType = property.PropertyType.GetGenericArguments()[0];

			listProvider.GetOrAddContentType(entityType, () => new AnnotatedContentTypeMapping(entityType));
		}

		private static string ResolveListTitle(PropertyInfo property)
		{
			var listAttribute = property.GetCustomAttribute<SpListAttribute>();

			return string.IsNullOrEmpty(listAttribute.Title) ? property.Name : listAttribute.Title;
		}
	}
}