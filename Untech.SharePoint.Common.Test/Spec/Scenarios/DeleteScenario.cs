using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec.Scenarios
{
	public class DeleteScenario<T> : ListScenario<T>
		where T : Entity
	{
		private readonly IValueGenerator<T> _itemGenerator;

		public DeleteScenario(ISpList<T> list, IValueGenerator<T> itemGenerator)
			: base(list)
		{
			_itemGenerator = itemGenerator;
		}

		public override void Run()
		{
			var addedItem = List.Add(_itemGenerator.Generate());

			Stopwatch.Start();
			List.Delete(addedItem);
			Stopwatch.Stop();

			var foundItem = List.FirstOrDefault(n => n.Id == addedItem.Id);
			Assert.IsNull(foundItem);
		}
	}
}