using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal class AnnotatedContextProvider<T> : IMetaContextProvider
	{
		public AnnotatedContextProvider()
		{
			ContextType = typeof (T);
			ListProviders = new Dictionary<string, AnnotatedListProvider>();

			FillListProviders();
		}

		public Type ContextType { get; private set; }

		public Dictionary<string, AnnotatedListProvider> ListProviders { get; private set; }

		public MetaContext GetMetaContext()
		{
			return new MetaContext(ListProviders.Values.ToList());
		}

		private void FillListProviders()
		{
			var listAttribute = typeof (SpListAttribute);
			var listType = typeof (ISpList<>);

			ContextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.IsDefined(listAttribute))
				.Where(n => n.CanRead)
				.Where(n => n.PropertyType.IsGenericType)
				.Where(n => listType == n.PropertyType.GetGenericTypeDefinition())
				.ToList()
				.ForEach(AddListContentType);
		}

		private void AddListContentType(PropertyInfo member)
		{
			var listAttribute = member.GetCustomAttribute<SpListAttribute>();

			if (!ListProviders.ContainsKey(listAttribute.ListTitle))
			{
				ListProviders.Add(listAttribute.ListTitle, new AnnotatedListProvider(listAttribute.ListTitle));
			}

			var provider = ListProviders[listAttribute.ListTitle];

			var modelType = member.PropertyType.GetGenericArguments()[0];

			if (provider.ContentTypeProviders.ContainsKey(modelType))
			{
				return;
			}

			provider.ContentTypeProviders.Add(modelType, new AnnotatedContentTypeProvider(modelType));
		}

	}
}