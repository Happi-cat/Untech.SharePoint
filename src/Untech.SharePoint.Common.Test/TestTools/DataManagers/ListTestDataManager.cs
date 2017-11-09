using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data;

namespace Untech.SharePoint.TestTools.DataManagers
{
	public class ListTestDataManager<T>
	{
		[NotNull]
		private readonly ISpList<T> _list;
		[NotNull]
		private List<T> _items = new List<T>();

		public ListTestDataManager([NotNull]ISpList<T> list)
		{
			_list = list;
		}

		public IReadOnlyList<T> GeneratedItems => _items;

		public void Load()
		{
			_items = _list.ToList();
		}
	}
}