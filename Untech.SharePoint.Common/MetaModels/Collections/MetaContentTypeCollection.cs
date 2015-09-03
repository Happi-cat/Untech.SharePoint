using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.SharePoint.Common.MetaModels.Collections
{
	public sealed class MetaContentTypeCollection : ReadOnlyDictionary<Type, MetaContentType>, IReadOnlyCollection<MetaContentType>
	{
		public MetaContentTypeCollection(IEnumerable<MetaContentType> enumerable) 
			: base(CreateDictionary(enumerable))
		{
		}

		IEnumerator<MetaContentType> IEnumerable<MetaContentType>.GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		private static IDictionary<Type, MetaContentType> CreateDictionary(IEnumerable<MetaContentType> enumerable)
		{
			return enumerable.ToDictionary(n => n.EntityType);
		}
	}
}