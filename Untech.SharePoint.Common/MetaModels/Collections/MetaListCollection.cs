using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.SharePoint.Common.MetaModels.Collections
{
	public sealed class MetaListCollection : ReadOnlyDictionary<string, MetaList>, IEnumerable<MetaList>
	{
		public MetaListCollection(IEnumerable<MetaList> enumerable)
			: base(CreateDictionary(enumerable))
		{
		}

		IEnumerator<MetaList> IEnumerable<MetaList>.GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		private static IDictionary<string, MetaList> CreateDictionary(IEnumerable<MetaList> enumerable)
		{
			return enumerable.ToDictionary(n => n.ListTitle);
		}
	}
}