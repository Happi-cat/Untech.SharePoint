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
			ListProviders = new Dictionary<string, IMetaListProvider>();

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

			var listProperties = ContextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.IsDefined(listAttribute))
				.Where(n => n.CanRead)
				.Where(n => n.PropertyType.IsGenericType)
				.Where(n => listType.IsAssignableFrom(n.PropertyType.GetGenericTypeDefinition()))
				.ToList();

			listProperties
				.Select(n => n.GetCustomAttribute<SpListAttribute>())
				.Select(n => n.ListTitle)
				.ToList()
				.ForEach(n => ListProviders.Add(n, new AnnotatedListProvider(n)));

		}

	}

	internal class AnnotatedListProvider : IMetaListProvider
	{
		public AnnotatedListProvider(string listTitle)
		{
			ListTitle = listTitle;
			ContentTypeProviders = new Dictionary<Type, AnnotatedContentTypeProvider>();
		}

		public string ListTitle { get; private set; }

		public Dictionary<Type, AnnotatedContentTypeProvider> ContentTypeProviders { get; private set; }


		public MetaList GetMetaList(MetaContext parent)
		{
			return new MetaList(parent, ListTitle, ContentTypeProviders.Values.ToList());
		}
	}
}