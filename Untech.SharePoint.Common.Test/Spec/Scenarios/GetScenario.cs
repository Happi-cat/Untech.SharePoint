using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec.Scenarios
{
	public class GetScenario<T> : ListScenario<T>
		where T : Entity
	{
		private readonly IValueGenerator<T> _itemGenerator;
		private T _addedItem;

		public GetScenario(ISpList<T> list, IValueGenerator<T> itemGenerator)
			: base(list)
		{
			_itemGenerator = itemGenerator;
		}

		public override void BeforeRun()
		{
			_addedItem = List.Add(_itemGenerator.Generate());
		}

		public override void Run()
		{
			Stopwatch.Start();
			var item = List.Get(_addedItem.Id);
			Stopwatch.Stop();

			Assert.IsNotNull(item);
			Assert.AreEqual(item.Id, _addedItem.Id);
		}

		public override void AfterRun()
		{
			List.Delete(_addedItem);
		}
	}
}