using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.SharePoint.Common.MetaModels.Collections
{
	public sealed class MetaContentTypeCollection : IReadOnlyCollection<MetaContentType>
	{
		private readonly ReadOnlyDictionary<Type, MetaContentType> _contentTypes;

		public MetaContentTypeCollection(IEnumerable<MetaContentType> contentTypes)
		{
			_contentTypes = new ReadOnlyDictionary<Type, MetaContentType>(contentTypes.ToDictionary(n => n.EntityType));
		}

		public IEnumerator<MetaContentType> GetEnumerator()
		{
			return _contentTypes.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count { get { return _contentTypes.Values.Count; } }

		public MetaContentType GetByEntityType(Type entityType)
		{
			return _contentTypes[entityType];
		}
	}
}