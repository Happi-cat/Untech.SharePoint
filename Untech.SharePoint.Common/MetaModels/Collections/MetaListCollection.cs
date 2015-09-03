using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.SharePoint.Common.MetaModels.Collections
{
	public sealed class MetaListCollection : IReadOnlyCollection<MetaList>
	{
		private readonly ReadOnlyDictionary<string, MetaList> _lists;

		public MetaListCollection(IEnumerable<MetaList> lists)
		{
			_lists = new ReadOnlyDictionary<string, MetaList>(lists.ToDictionary(n => n.ListTitle));
		}

		public IEnumerator<MetaList> GetEnumerator()
		{
			return _lists.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count { get { return _lists.Values.Count; } }

		public MetaList GetByListTitle(string listTitle)
		{
			return _lists[listTitle];
		}
	}
}