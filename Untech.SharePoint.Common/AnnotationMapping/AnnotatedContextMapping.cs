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
		private Dictionary<string, AnnotatedListMapping> _listProviders;

		public AnnotatedContextMapping()
		{
			ContextType = typeof(T);

			RegisterListProviders();
		}

		public Type ContextType { get; private set; }

		public IReadOnlyDictionary<string, AnnotatedListMapping> ListProviders { get { return _listProviders; } }


		public MetaContext GetMetaContext()
		{
			return new MetaContext(ListProviders.Values.ToList());
		}

		public AnnotatedListMapping GetOrAddList(string listTitle)
		{
			if (!_listProviders.ContainsKey(listTitle))
			{
				_listProviders.Add(listTitle, new AnnotatedListMapping(listTitle));
			}

			return _listProviders[listTitle];
		}

		private void RegisterListProviders()
		{
			_listProviders = new Dictionary<string, AnnotatedListMapping>();

			var attributeType = typeof(SpListAttribute);
			var listType = typeof(ISpList<>);

			ContextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Where(n => n.CanRead)
				.Where(n => n.PropertyType.IsGenericType && n.PropertyType.GetGenericTypeDefinition() == listType)
				.Each(RegisterListProvider);
		}

		private void RegisterListProvider(PropertyInfo property)
		{
			var listProvider = GetOrAddList(ResolveListTitle(property));

			var entityType = property.PropertyType.GetGenericArguments()[0];

			listProvider.GetOrAddContentType(entityType, () => new AnnotatedContentTypeMapping(entityType));
		}

		private static string ResolveListTitle(PropertyInfo property)
		{
			var listAttribute = property.GetCustomAttribute<SpListAttribute>();

			return string.IsNullOrEmpty(listAttribute.ListTitle)
				? property.Name
				: listAttribute.ListTitle;
		}
	}
}