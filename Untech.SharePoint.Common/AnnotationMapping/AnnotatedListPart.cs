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
	internal class AnnotatedListPart : IMetaListProvider
	{
		private readonly string _title;
		private readonly Dictionary<Type, AnnotatedContentTypeMapping> _contentTypeProviders;

		protected AnnotatedListPart(string listTitle)
		{
			Guard.CheckNotNullOrEmpty("listTitle", listTitle);

			_title = listTitle;
			_contentTypeProviders = new Dictionary<Type, AnnotatedContentTypeMapping>();
		}

		#region [Public Static]

		public static bool IsAnnotated(PropertyInfo property)
		{
			return property.IsDefined(typeof (SpListAttribute));
		}

		public static AnnotatedListPart Create(string listTitle, IEnumerable<PropertyInfo> contextProperties)
		{
			var listMapping = new AnnotatedListPart(listTitle);

			contextProperties.Each(listMapping.RegisterContentType);

			return listMapping;
		}

		#endregion


		public MetaList GetMetaList(MetaContext parent)
		{
			return new MetaList(parent, _title, _contentTypeProviders.Values.ToList());
		}

		#region [Private Methods]

		private void RegisterContentType(PropertyInfo contextProperty)
		{
			if (!contextProperty.CanRead)
			{
				throw new InvalidAnnotationException(string.Format("Property {0} from {1} should be readable", contextProperty.Name,
					contextProperty.DeclaringType));
			}

			if (!contextProperty.PropertyType.IsGenericType ||
			    contextProperty.PropertyType.GetGenericTypeDefinition() != typeof (ISpList<>))
			{
				throw new InvalidAnnotationException(string.Format("Property {0} from {1} should have 'ISpList<T>' type",
					contextProperty.Name, contextProperty.DeclaringType));
			}

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