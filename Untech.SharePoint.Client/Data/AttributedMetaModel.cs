using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class AttributedMetaModel : MetaModel
	{
		private ReadOnlyCollection<MetaList> _lists;

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
				.Select(GetMetaList)
				.ToList();

			_lists = metaLists.AsReadOnly();
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