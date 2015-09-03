using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedContextProvider<T> : IMetaContextProvider
	{
		public AnnotatedContextProvider()
		{
			ContextType = typeof(T);
			ListProviders = new Dictionary<string, AnnotatedListProvider>();

			Init();
		}

		public Type ContextType { get; private set; }

		public Dictionary<string, AnnotatedListProvider> ListProviders { get; private set; }


		public MetaContext GetMetaContext()
		{
			return new MetaContext(ListProviders.Values.ToList());
		}

		public AnnotatedListProvider GetOrAddList(string listTitle)
		{
			if (!ListProviders.ContainsKey(listTitle))
			{
				ListProviders.Add(listTitle, new AnnotatedListProvider(listTitle));
			}

			return ListProviders[listTitle];
		}

		private void Init()
		{
			var attributeType = typeof(SpListAttribute);
			var listType = typeof(ISpList<>);

			ContextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Where(n => n.CanRead)
				.Where(n => n.PropertyType.IsGenericType && n.PropertyType.GetGenericTypeDefinition() == listType)
				.ToList()
				.ForEach(RegisterListContentType);
		}

		private void RegisterListContentType(PropertyInfo member)
		{
			var listAttribute = member.GetCustomAttribute<SpListAttribute>();
			var listProvider = GetOrAddList(listAttribute.ListTitle);

			var modelType = member.PropertyType.GetGenericArguments()[0];
			listProvider.GetOrAddContentType(modelType, () => new AnnotatedContentTypeProvider(modelType));
		}

	}
}