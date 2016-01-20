using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.DataGenerators;
using Untech.SharePoint.Common.Test.Tools.Generators;
using Untech.SharePoint.Common.Test.Tools.Generators.Basic;

namespace Untech.SharePoint.Common.Test.Spec
{
	public class BasicOperationsSpec
	{
		private readonly IDataContext _dataContext;
		private readonly string _token;
		private readonly DateTime _date1;
		private readonly DateTime _date2;

		public BasicOperationsSpec(string token, IDataContext dataContext)
		{
			_date1 = DateTime.Now.AddMinutes(-1);
			_date2 = DateTime.Now.AddMinutes(1);
			_token = token;
			_dataContext = dataContext;
		}

		public void AddUpdateDelete()
		{
			var addedItem = Add();
			Update(addedItem);
			Delete(addedItem);
		}

		public EventModel Add()
		{
			var itemToAdd = GenerateModel();
			var addedItem = _dataContext.Events.Add(itemToAdd);

			Assert.IsTrue(addedItem.Id > 0);
			Assert.AreEqual(itemToAdd.Title, addedItem.Title);

			return addedItem;
		}

		public void Update(EventModel existingItem)
		{
			existingItem.Title += "[Updated]";

			_dataContext.Events.Update(existingItem);

			var updatedItem = _dataContext.Events.Get(existingItem.Id);

			Assert.AreEqual(existingItem.Id, updatedItem.Id);
			Assert.AreEqual(existingItem.Title, updatedItem.Title);
		}

		public void Delete(EventModel existingItem)
		{
			_dataContext.Events.Delete(existingItem);

			var foundItem = _dataContext.Events.FirstOrDefault(n => n.Id == existingItem.Id);

			Assert.IsNull(foundItem);
		}
		public void BatchAddUpdateDelete()
		{
			var addedItems = AddBatch();
			UpdateBatch(addedItems);
			DeleteBatch(addedItems);
		}

		public List<EventModel> AddBatch()
		{
			var itemsToAdd = GenerateModels();
			itemsToAdd.Each(AddToken);

			_dataContext.Events.Add(itemsToAdd);

			var addedItems = GetExistingItems();

			var generatedTitles = itemsToAdd.Select(n => n.Title);
			var addedTitles = addedItems.Select(n => n.Title);

			Assert.IsTrue(generatedTitles.SequenceEqual(addedTitles));

			return addedItems;
		}

		public void UpdateBatch(List<EventModel> existingItems)
		{
			existingItems.Each(n => n.Title += "[Updated]");

			_dataContext.Events.Update(existingItems);

			var updatedItems = GetExistingItems();

			var generatedTitles = existingItems.Select(n => n.Title);
			var updatedTitles = updatedItems.Select(n => n.Title);

			Assert.IsTrue(generatedTitles.SequenceEqual(updatedTitles));
		}

		public void DeleteBatch(List<EventModel> existingItems)
		{
			_dataContext.Events.Delete(existingItems);

			var deletedItems = GetExistingItems();

			Assert.IsFalse(deletedItems.Any());
		}

		private List<EventModel> GetExistingItems()
		{
			return _dataContext.Events
				.Where(n => n.Title.StartsWith("<" + _token + ">"))
				.Where(n => n.WhenStart == _date1 && n.WhenComplete == _date2)
				.ToList();
		}

		private IValueGenerator<EventModel> GetGenerator()
		{
			return Generators.GetGoingEventGenerator()
				.WithStatic(n => n.WhenStart, _date1)
				.WithStatic(n => n.WhenComplete, _date2);
		}

		private EventModel GenerateModel()
		{
			return GetGenerator().Generate();
		}

		private List<EventModel> GenerateModels()
		{
			var generator = new ArrayGenerator<EventModel>(GetGenerator())
			{
				Size = 50
			};
			return generator.Generate();
		}

		private void AddToken(EventModel model)
		{
			model.Title = "<" + _token + ">: " + model.Title;
		}
	}
}