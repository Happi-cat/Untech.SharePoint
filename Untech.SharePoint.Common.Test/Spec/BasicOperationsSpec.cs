using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Models;
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
			var addedEvent = Add(_dataContext.Events, GetEventGenerator());
			Update(_dataContext.Events, addedEvent);
			Delete(_dataContext.Events, addedEvent);

			var addedProject = Add(_dataContext.Projects, GetProjectGenerator());
			Update(_dataContext.Projects, addedProject);
			Delete(_dataContext.Projects, addedProject);
		}

		public T Add<T>(ISpList<T> list, IValueGenerator<T> generator)
			where T: Entity
		{
			var itemToAdd = generator.Generate();
			var addedItem = list.Add(itemToAdd);

			Assert.IsTrue(addedItem.Id > 0, "addedItem.Id > 0");
			Assert.AreEqual(itemToAdd.Title, addedItem.Title, "Titles are not equal");

			Assert.IsTrue(addedItem.Created > DateTime.Today, "addedItem.Created > DateTime.Today");
			Assert.IsTrue(addedItem.Author != null && addedItem.Author.Id > 0, "addedItem.Author.Id > 0");

			Assert.IsTrue(addedItem.Modified > DateTime.Today, "addedItem.Modified > DateTime.Today");
			Assert.IsTrue(addedItem.Editor != null && addedItem.Editor.Id > 0, "addedItem.Editor.Id > 0");

			return addedItem;
		}

		public void Update<T>(ISpList<T> list, T existingItem)
			where T : Entity
		{
			existingItem.Title += "[Updated]";

			Thread.Sleep(1000);
			var updatedItem = list.Update(existingItem);

			Assert.AreEqual(existingItem.Id, updatedItem.Id, "Ids are not equal");
			Assert.AreEqual(existingItem.Title, updatedItem.Title, "Titles are not equal");

			Assert.IsTrue(updatedItem.Modified > existingItem.Modified, "updatedItem.Modified > existingItem.Modified");
		}

		public void Delete<T>(ISpList<T> list, T existingItem)
			where T : Entity
		{
			list.Delete(existingItem);

			var foundItem = list.FirstOrDefault(n => n.Id == existingItem.Id);

			Assert.IsNull(foundItem, "foundItem != null");
		}

		public void BatchAddUpdateDelete()
		{
			var addedEvents = AddBatch(_dataContext.Events, GetEventGenerator(), GetExistingItems);
			UpdateBatch(_dataContext.Events, addedEvents, GetExistingItems);
			DeleteBatch(_dataContext.Events, addedEvents, GetExistingItems);

			var addedProjects = AddBatch(_dataContext.Projects, GetProjectGenerator(), GetExistingItems);
			UpdateBatch(_dataContext.Projects, addedProjects, GetExistingItems);
			DeleteBatch(_dataContext.Projects, addedProjects, GetExistingItems);
		}

		public List<T> AddBatch<T>(ISpList<T> list, IValueGenerator<T> itemGenerator, Func<ISpList<T>, List<T>> selector)
			where T : Entity
		{
			var arrayGenerator = new ArrayGenerator<T>(itemGenerator) { Size = 50 };

			var itemsToAdd = arrayGenerator.Generate();
			itemsToAdd.Each(AddToken);

			list.Add(itemsToAdd);

			var addedItems = selector(list);

			var generatedTitles = itemsToAdd.Select(n => n.Title);
			var addedTitles = addedItems.Select(n => n.Title);

			Assert.IsTrue(generatedTitles.SequenceEqual(addedTitles), "generatedTitles.SequenceEqual(addedTitles)");

			return addedItems;
		}

		public void UpdateBatch<T>(ISpList<T> list, List<T> existingItems, Func<ISpList<T>, List<T>> selector)
			where T : Entity
		{
			existingItems.Each(n => n.Title += "[Updated]");

			list.Update(existingItems);

			var updatedItems = selector(list);

			var generatedTitles = existingItems.Select(n => n.Title);
			var updatedTitles = updatedItems.Select(n => n.Title);

			Assert.IsTrue(generatedTitles.SequenceEqual(updatedTitles), "generatedTitles.SequenceEqual(updatedTitles)");
		}

		public void DeleteBatch<T>(ISpList<T> list, List<T> existingItems, Func<ISpList<T>, List<T>> selector)
			where T : Entity
		{
			list.Delete(existingItems);

			var deletedItems = selector(list);

			Assert.IsFalse(deletedItems.Any(), "deletedItems.Any()");
		}

		private List<ProjectModel> GetExistingItems(ISpList<ProjectModel> projects)
		{
			return projects
				.Where(n => n.Title.StartsWith("<" + _token + ">"))
				.ToList();
		}

		private List<EventModel> GetExistingItems(ISpList<EventModel> events)
		{
			return events
				.Where(n => n.Title.StartsWith("<" + _token + ">"))
				.Where(n => n.WhenStart == _date1 && n.WhenComplete == _date2)
				.ToList();
		}

		private IValueGenerator<EventModel> GetEventGenerator()
		{
			return Generators.GetGoingEventGenerator()
				.WithStatic(n => n.WhenStart, _date1)
				.WithStatic(n => n.WhenComplete, _date2);
		}

		private IValueGenerator<ProjectModel> GetProjectGenerator()
		{
			return Generators.GetProjectGenerator();
		}

		private void AddToken(Entity model)
		{
			model.Title = "<" + _token + ">: " + model.Title;
		}
	}
}