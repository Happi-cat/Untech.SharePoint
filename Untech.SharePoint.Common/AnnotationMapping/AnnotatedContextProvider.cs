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
	internal sealed class AnnotatedContextProvider<T> : IMetaContextProvider
	{
		private readonly Dictionary<string, AnnotatedListProvider> _listProviders;

		public AnnotatedContextProvider()
		{
			ContextType = typeof(T);

			_listProviders = new Dictionary<string, AnnotatedListProvider>();

			RegisterLists();
		}

		public Type ContextType { get; private set; }

		public IReadOnlyDictionary<string, AnnotatedListProvider> ListProviders { get { return _listProviders; } }


		public MetaContext GetMetaContext()
		{
			return new MetaContext(ListProviders.Values.ToList());
		}

		public AnnotatedListProvider GetOrAddList(string listTitle)
		{
			if (!_listProviders.ContainsKey(listTitle))
			{
				_listProviders.Add(listTitle, new AnnotatedListProvider(listTitle));
			}

			return _listProviders[listTitle];
		}

		private void RegisterLists()
		{
			var attributeType = typeof(SpListAttribute);
			var listType = typeof(ISpList<>);

			ContextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Where(n => n.CanRead)
				.Where(n => n.PropertyType.IsGenericType && n.PropertyType.GetGenericTypeDefinition() == listType)
				.Each(RegisterListWithContentType);
		}

		private void RegisterListWithContentType(PropertyInfo member)
		{
			var listAttribute = member.GetCustomAttribute<SpListAttribute>();
			var listTitle = string.IsNullOrEmpty(listAttribute.ListTitle) ? member.Name : listAttribute.ListTitle;

			var listProvider = GetOrAddList(listTitle);

			var modelType = member.PropertyType.GetGenericArguments()[0];

			listProvider.GetOrAddContentType(modelType, () => new AnnotatedContentTypeProvider(modelType));
		}

	}
}