using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Data.FieldConverters;
using System;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaModel
	{
		public abstract MetaList GetList(string listTitle, Type itemType);

		public abstract IEnumerable<MetaList> GetLists();

		public override string ToString()
		{
			return string.Format("( Lists=[ {0} ]; )", GetLists().JoinToString());
		}
	}

	internal sealed class SpList<T>
	{

	}

	internal sealed class AttributedMetaModel : MetaModel
	{
		private ReadOnlyCollection<MetaList> _lists;

		public AttributedMetaModel(Type dataContextType)
		{
			DataContextType = dataContextType;
		}

		public Type DataContextType { get; private set; }

		private void InitMetaList()
		{
			var metaLists = DataContextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.PropertyType.IsGenericType && n.PropertyType.GetGenericTypeDefinition() == listGenericType)
				.Select(GetMetaList)
				.ToList();

			_lists = metaLists;
		}

		private MetaList GetMetaList(PropertyInfo property)
		{
			var listAttribute = property.GetCustomAttribute<SpListAttribute>() ?? new SpListAttribute(property.Name);
			listAttribute.ListTitle = listAttribute.ListTitle ?? property.Name;

			var itemType = property.PropertyType.GetGenericArguments()[0];

			return new AttributedMetaList(this, listAttribute, itemType, null);
		}


		public override MetaList GetList(string listTitle, Type itemType)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<MetaList> GetLists()
		{
			throw new NotImplementedException();
		}
	}
}