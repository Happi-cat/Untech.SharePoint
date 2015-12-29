using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec.Scenarios
{
	public class UpdateScenario<T> : ListScenario<T>
		where T : Entity
	{
		private readonly IValueGenerator<string> _titleGenerator;
		private readonly IValueGenerator<T> _itemGenerator;
		private T _addedItem;

		public UpdateScenario(ISpList<T> list, IValueGenerator<T> itemGenerator)
			: base(list)
		{
			_titleGenerator = new LoremGenerator {MinWords = 5, MaxWords = 10};
			_itemGenerator = itemGenerator;
		}

		public override void BeforeRun()
		{
			_addedItem = List.Add(_itemGenerator.Generate());
		}

		public override void Run()
		{
			var oldTitle = _addedItem.Title;
			var generatedTitle = string.Format("Upd #{0}: {1}", DateTime.Now.Ticks, _titleGenerator.Generate());
			_addedItem.Title = generatedTitle;

			Stopwatch.Start();
			List.Update(_addedItem);
			Stopwatch.Stop();

			var updateItem = List.Get(_addedItem.Id);
			Assert.AreNotEqual(updateItem.Title, oldTitle);
			Assert.AreEqual(updateItem.Title, generatedTitle);
		}

		public override void AfterRun()
		{
			List.Delete(_addedItem);
		}
	}
}