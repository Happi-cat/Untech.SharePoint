using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.TestTools.Generators;
using Untech.SharePoint.Common.TestTools.Generators.Basic;

namespace Untech.SharePoint.Common.TestTools.DataGenerators
{
	public class ListTestDataGenerator<T>
	{
		[NotNull]
		private readonly List<ArrayGenerator<T>> _itemGenerators = new List<ArrayGenerator<T>>();
		[NotNull]
		private readonly ISpList<T> _list;
		[NotNull]
		private readonly List<T> _addedItems = new List<T>();

		public ListTestDataGenerator([NotNull]ISpList<T> list)
		{
			_list = list;
		}

		public IReadOnlyList<T> GeneratedItems { get { return _addedItems; } }

		public void Generate()
		{
			var addedItems = _itemGenerators
				   .SelectMany(n => n.Generate())
				   .Select(n => _list.Add(n))
				   .ToList();

			_itemGenerators.Clear();
			_addedItems.AddRange(addedItems);
		}

		public ListTestDataGenerator<T> WithArray(int size, IValueGenerator<T> item)
		{
			_itemGenerators.Add(new ArrayGenerator<T>(item) { Size = size });

			return this;
		}
	}
}