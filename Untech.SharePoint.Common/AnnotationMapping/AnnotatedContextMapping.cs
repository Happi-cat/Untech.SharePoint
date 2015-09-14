using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.Services;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedContextMapping<T> : IMetaContextProvider, IListTitleResolver
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
			ValidateList(property);

			var listProvider = GetOrAddList(GetListTitleFromContextProperty(property));

			var entityType = property.PropertyType.GetGenericArguments()[0];

			listProvider.GetOrAddContentType(entityType, () => AnnotatedContentTypeMapping.Create(entityType));
		}

		public static void ValidateList(PropertyInfo property)
		{
			if (!property.CanRead)
			{
				throw new AnnotationException(string.Format("Property {0} from {1} should be readable", property.Name, property.DeclaringType));
			}

			if (!property.PropertyType.IsGenericType || property.PropertyType.GetGenericTypeDefinition() != typeof(ISpList<>))
			{
				throw new AnnotationException(string.Format("Property {0} from {1} should have 'ISpList<T>' type", property.Name, property.DeclaringType));
			}
		}

		public string GetListTitleFromContextProperty(PropertyInfo property)
		{
			var listAttribute = property.GetCustomAttribute<SpListAttribute>();

			return string.IsNullOrEmpty(listAttribute.Title) ? property.Name : listAttribute.Title;
		}
	}
}