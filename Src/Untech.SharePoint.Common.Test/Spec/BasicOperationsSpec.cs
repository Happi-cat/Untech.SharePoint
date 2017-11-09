using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Data;
using Untech.SharePoint.Extensions;
using Untech.SharePoint.Models;
using Untech.SharePoint.Spec.Models;
using Untech.SharePoint.TestTools.DataGenerators;
using Untech.SharePoint.TestTools.Generators;
using Untech.SharePoint.TestTools.Generators.Basic;

namespace Untech.SharePoint.Spec
{
	public class BasicOperationsSpec
	{
		private readonly DataContext _dataContext;
		private readonly string _token;
		private readonly DateTime _date1;
		private readonly DateTime _date2;

		public BasicOperationsSpec(string token, DataContext dataContext)
		{
			_date1 = DateTime.Now.AddMinutes(-1);
			_date2 = DateTime.Now.AddMinutes(1);
			_token = token;
			_dataContext = dataContext;
		}

		public void AddUpdateDelete()
		{
			var addedEvent = Add(_dataContext.Events, GetEventGenerator());
			Thread.Sleep(1000);
			Update(_dataContext.Events, addedEvent);
			Delete(_dataContext.Events, addedEvent);

			var addedNews = Add(_dataContext.News, GetNewsGenerator());
			Thread.Sleep(1000);
			Update(_dataContext.News, addedNews);
			Delete(_dataContext.News, addedNews);

			var addedTeam = Add(_dataContext.Teams, GetTeamGenerator());
			Thread.Sleep(1000);
			Update(_dataContext.Teams, addedTeam);
			Delete(_dataContext.Teams, addedTeam);

			var addedProject = Add(_dataContext.Projects, GetProjectGenerator());
			Thread.Sleep(1000);
			Update(_dataContext.Projects, addedProject);
			Delete(_dataContext.Projects, addedProject);
		}

		public T Add<T>(ISpList<T> list, IValueGenerator<T> generator)
			where T : Entity
		{
			var now = TrimMilliseconds(DateTime.Now);

			var itemToAdd = generator.Generate();
			var addedItem = list.Add(itemToAdd);

			Assert.IsTrue(addedItem.Id > 0, "addedItem.Id > 0");
			Assert.AreEqual(itemToAdd.Title, addedItem.Title, "Titles are not equal");

			Assert.IsTrue(addedItem.Created >= now, "addedItem.Created >= DateTime.Now");
			Assert.IsTrue(addedItem.Author != null && addedItem.Author.Id > 0, "addedItem.Author.Id > 0");

			Assert.IsTrue(addedItem.Modified >= now, "addedItem.Modified >= DateTime.Now");
			Assert.IsTrue(addedItem.Editor != null && addedItem.Editor.Id > 0, "addedItem.Editor.Id > 0");

			return addedItem;
		}

		public void Update<T>(ISpList<T> list, T existingItem)
			where T : Entity
		{
			existingItem.Title += "[Updated]";

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

			var addedNews = AddBatch(_dataContext.News, GetNewsGenerator(), GetExistingItems);
			UpdateBatch(_dataContext.News, addedNews, GetExistingItems);
			DeleteBatch(_dataContext.News, addedNews, GetExistingItems);

			var addedTeams = AddBatch(_dataContext.Teams, GetTeamGenerator(), GetExistingItems);
			UpdateBatch(_dataContext.Teams, addedTeams, GetExistingItems);
			DeleteBatch(_dataContext.Teams, addedTeams, GetExistingItems);

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

		private List<NewsModel> GetExistingItems(ISpList<NewsModel> news)
		{
			return news
				.Where(n => n.Title.StartsWith("<" + _token + ">"))
				.ToList();
		}

		private List<TeamModel> GetExistingItems(ISpList<TeamModel> teams)
		{
			return teams
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
			return Generators.GetProjectGenerator()
				.WithStatic(n => n.Team, new ObjectReference { Id = 1 });
		}

		private IValueGenerator<NewsModel> GetNewsGenerator()
		{
			return Generators.GetNewsGenerator();
		}

		private IValueGenerator<TeamModel> GetTeamGenerator()
		{
			return Generators.GetTeamGenerator()
				.WithStatic(n => n.ProjectManager, new UserInfo { Id = 1 })
				.WithStatic(n => n.FinanceManager, new UserInfo { Id = 1 });
		}

		private void AddToken(Entity model)
		{
			model.Title = "<" + _token + ">: " + model.Title;
		}

		private DateTime TrimMilliseconds(DateTime dateTime)
		{
			return dateTime.AddTicks(-dateTime.Ticks % 10000000);
		}
	}
}