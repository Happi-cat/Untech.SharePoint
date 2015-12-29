using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec.Scenarios
{
	public class AddScenario<T> : ListScenario<T>
		where T : Entity
	{
		private readonly IValueGenerator<T> _itemGenerator;

		public AddScenario(ISpList<T> list, IValueGenerator<T> itemGenerator)
			: base(list)
		{
			_itemGenerator = itemGenerator;
		}

		public override void Run()
		{
			Stopwatch.Start();
			var addedItem = List.Add(_itemGenerator.Generate());
			Stopwatch.Stop();

			Assert.IsTrue(addedItem.Id > 0);

			List.Delete(addedItem);
		}
	}
}