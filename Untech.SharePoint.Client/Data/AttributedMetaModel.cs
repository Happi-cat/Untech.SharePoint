using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class AttributedMetaModel : MetaModel
	{
		private ReadOnlyCollection<MetaList> _lists;
		private ReadOnlyDictionary<MemberInfo, MetaList> _membersListsMap;

		public AttributedMetaModel(Type dataContextType, ISpFieldsResolver resolver)
		{
			Guard.CheckNotNull("dataContextType", dataContextType);

			DataContextType = dataContextType;
			FieldsResolver = resolver;
			
			InitMetaList();
		}

		public ISpFieldsResolver FieldsResolver{ get; private set; }

		public Type DataContextType { get; private set; }

		private void InitMetaList()
		{
			var listGenericType = typeof(SpList<>);

			var metaLists = DataContextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.PropertyType.IsGenericType && n.PropertyType.GetGenericTypeDefinition() == listGenericType)
				.Select(n => new KeyValuePair<PropertyInfo, MetaList>(n, GetMetaList(n)))
				.ToList();

			_lists = metaLists.Select(n=>n.Value).ToList().AsReadOnly();
			_membersListsMap =
				new ReadOnlyDictionary<MemberInfo, MetaList>(metaLists.ToDictionary(n => (MemberInfo) n.Key, n => n.Value));
		}

		private MetaList GetMetaList(PropertyInfo property)
		{
			var listAttribute = property.GetCustomAttribute<SpListAttribute>();
			if (listAttribute == null || string.IsNullOrEmpty(listAttribute.ListTitle))
			{
				listAttribute = new SpListAttribute(property.Name);
			}

			var itemType = property.PropertyType.GetGenericArguments()[0];

			return new AttributedMetaList(this, listAttribute, itemType, FieldsResolver);
		}


		public override MetaList GetList(MemberInfo memberInfo)
		{
			return _membersListsMap[memberInfo];
		}

		public override MetaList GetList(string listTitle, Type itemType)
		{
			return _lists.FirstOrDefault(n => n.ListTitle == listTitle && n.ItemType.Type == itemType);
		}

		public override IEnumerable<MetaList> GetLists()
		{
			return _lists;
		}
	}
}