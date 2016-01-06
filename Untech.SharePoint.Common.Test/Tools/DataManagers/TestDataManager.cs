using System.Collections.Generic;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Common.Test.Tools.DataManagers
{
	public class TestDataManager
	{
		private readonly ListTestDataManager<NewsModel> _newsData;
		private readonly ListTestDataManager<EventModel> _eventsData;
		private readonly ListTestDataManager<TeamModel> _teamsData;
		private readonly ListTestDataManager<ProjectModel> _projectsData;

		public TestDataManager(IDataContext dataContext)
		{
			_newsData = new ListTestDataManager<NewsModel>(dataContext.News);
			_eventsData = new ListTestDataManager<EventModel>(dataContext.Events);
			_teamsData = new ListTestDataManager<TeamModel>(dataContext.Teams);
			_projectsData = new ListTestDataManager<ProjectModel>(dataContext.Projects);
		}

		public IReadOnlyList<NewsModel> News { get { return _newsData.GeneratedItems; } }
		public IReadOnlyList<EventModel> Events { get { return _eventsData.GeneratedItems; } }
		public IReadOnlyList<TeamModel> Teams { get { return _teamsData.GeneratedItems; } }
		public IReadOnlyList<ProjectModel> Projects { get { return _projectsData.GeneratedItems; } }

		public void Init()
		{
			_newsData.Load();
			_eventsData.Load();
			_teamsData.Load();
			_projectsData.Load();
		}

		
	}
}