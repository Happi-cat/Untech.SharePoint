using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec.Scenarios
{
	public class ConverterScenario<T> : ListScenario<T>
		where T : Entity
	{
		private readonly IValueGenerator<T> _itemGenerator;
		private readonly IEqualityComparer<T> _comparer;
		private T _generatedItem;
		private T _addedItem;


		public ConverterScenario(ISpList<T> list, IValueGenerator<T> itemGenerator, IEqualityComparer<T> comparer)
			: base(list)
		{
			_itemGenerator = itemGenerator;
			_comparer = comparer;
		}

		public override void BeforeRun()
		{
			_generatedItem = _itemGenerator.Generate();
			_addedItem = List.Add(_generatedItem);
		}

		public override void Run()
		{
			var loadedItem = List.Get(_addedItem.Id);

			Assert.IsNotNull(loadedItem);
			Assert.IsTrue(_comparer.Equals(_generatedItem, _addedItem));
			Assert.IsTrue(_comparer.Equals(_addedItem, loadedItem));
		}

		public override void AfterRun()
		{
			List.Delete(_addedItem);
		}
	}
}