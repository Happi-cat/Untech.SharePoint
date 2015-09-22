using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	internal class AnnotatedContextMapping<T> : IMetaContextProvider, IListTitleResolver
	{
		private readonly Type _contextType;
		private readonly List<AnnotatedListPart> _listProviders;

		public AnnotatedContextMapping()
		{
			_contextType = typeof(T);
			_listProviders = CreateListParts();
		}

		public string GetListTitleFromContextMember(MemberInfo member)
		{
			var listAttribute = member.GetCustomAttribute<SpListAttribute>();

			return string.IsNullOrEmpty(listAttribute.Title) ? member.Name : listAttribute.Title;
		}

		public MetaContext GetMetaContext()
		{
			return new MetaContext(_listProviders);
		}

		#region [Private Methods]

		private List<AnnotatedListPart> CreateListParts()
		{
			return _contextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(AnnotatedListPart.IsAnnotated)
				.GroupBy(GetListTitleFromContextMember)
				.Select(CreateListPart)
				.ToList();
		}

		private static AnnotatedListPart CreateListPart(IGrouping<string, PropertyInfo> listProperties)
		{
			return AnnotatedListPart.Create(listProperties.Key, listProperties);
		}

		#endregion

	}
}