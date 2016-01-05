using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Tools.Generators;
using Untech.SharePoint.Common.Test.Tools.Generators.Basic;

namespace Untech.SharePoint.Common.Test.Tools.DataManagers
{
	public class ListTestDataManager<T> : IDisposable
	{
		[NotNull]
		private readonly List<ArrayGenerator<T>> _itemGenerators = new List<ArrayGenerator<T>>();
		[NotNull]
		private readonly ISpList<T> _list;
		[NotNull]
		private readonly List<T> _addedItems = new List<T>();

		public ListTestDataManager([NotNull]ISpList<T> list)
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

		public ListTestDataManager<T> WithArray(int size, IValueGenerator<T> item)
		{
			_itemGenerators.Add(new ArrayGenerator<T>(item) { Size = size });

			return this;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_addedItems.Each(n => _list.Delete(n));
			_addedItems.Clear();
		}
	}
}