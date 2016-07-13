using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Tools.DataManagers
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