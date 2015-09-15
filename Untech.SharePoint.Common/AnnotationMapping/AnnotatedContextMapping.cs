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

		public string GetListTitleFromContextProperty(PropertyInfo property)
		{
			var listAttribute = property.GetCustomAttribute<SpListAttribute>();

			return string.IsNullOrEmpty(listAttribute.Title) ? property.Name : listAttribute.Title;
		}

		public MetaContext GetMetaContext()
		{
			return new MetaContext(_listProviders.Values.ToList());
		}
		
		private void Initialize()
		{
			_contextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(AnnotatedListMapping.IsAnnotated)
				.Each(Register);
		}

		private void Register(PropertyInfo property)
		{
			GetOrAddList(property).RegisterFromContextProperty(property);
		}

		private AnnotatedListMapping GetOrAddList(PropertyInfo property)
		{
			var listTitle = GetListTitleFromContextProperty(property);

			if (!_listProviders.ContainsKey(listTitle))
			{
				_listProviders.Add(listTitle, AnnotatedListMapping.Create(listTitle, property));
			}

			return _listProviders[listTitle];
		}
	}
}