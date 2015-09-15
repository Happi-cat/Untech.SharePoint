using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Data;
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

		public static bool IsAnnotated(PropertyInfo property)
		{
			return property.IsDefined(typeof(SpListAttribute));
		}

		public static AnnotatedListMapping Create(string listTitle, PropertyInfo property)
		{
			if (!property.CanRead)
			{
				throw new AnnotationException(string.Format("Property {0} from {1} should be readable", property.Name, property.DeclaringType));
			}

			if (!property.PropertyType.IsGenericType || property.PropertyType.GetGenericTypeDefinition() != typeof(ISpList<>))
			{
				throw new AnnotationException(string.Format("Property {0} from {1} should have 'ISpList<T>' type", property.Name, property.DeclaringType));
			}

			return new AnnotatedListMapping(listTitle);
		}

		public void RegisterFromContextProperty(PropertyInfo property)
		{
			var entityType = property.PropertyType.GetGenericArguments()[0];

			Register(entityType);
		}

		public void Register(Type entityType)
		{
			if (!_contentTypeProviders.ContainsKey(entityType))
			{
				_contentTypeProviders.Add(entityType, AnnotatedContentTypeMapping.Create(entityType));
			}
		}

		public MetaList GetMetaList(MetaContext parent)
		{
			return new MetaList(parent, _title, _contentTypeProviders.Values.ToList());
		}
	}
}