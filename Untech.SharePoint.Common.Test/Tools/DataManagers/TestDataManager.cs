using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Spec.Models;
using Untech.SharePoint.Common.Test.Tools.Generators;
using Untech.SharePoint.Common.Test.Tools.Generators.Basic;

namespace Untech.SharePoint.Common.Test.Tools.DataManagers
{
	public class TestDataManager : IDisposable
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
			GenerateNews();
			GenerateEvents();
			GenerateTeams();
			GenerateProjects();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_newsData.Dispose();
			_eventsData.Dispose();
			_teamsData.Dispose();
			_projectsData.Dispose();
		}

		private void GenerateNews()
		{
			_newsData
				.WithArray(20, Spec.Models.Generators.GetNewsGenerator())
				.WithArray(20, Spec.Models.Generators.GetNewsGenerator()
					.WithRange(x => x.Description, new[] {"DESCRIPTION 1", "DESCRIPTION 2", "DESCRIPTION 3", "DESCRIPTION 4"}))
				.WithArray(20, Spec.Models.Generators.GetNewsGenerator()
					.WithStatic(x => x.Description, "STATIC"))
				.WithArray(1, Spec.Models.Generators.GetNewsGenerator()
					.WithStatic(x => x.Description, "SINGLETON"))
				.Generate();
		}

		private void GenerateEvents()
		{
			_eventsData
				.WithArray(20, Spec.Models.Generators.GetCompletedEventGenerator())
				.WithArray(20, Spec.Models.Generators.GetGoingEventGenerator())
				.WithArray(20, Spec.Models.Generators.GetFutureEventGenerator())
				.Generate();
		}

		private void GenerateTeams()
		{
			_teamsData
				.WithArray(20, Spec.Models.Generators.GetTeamGenerator())
				.Generate();
		}

		private void GenerateProjects()
		{
			GenerateProjects1();
			GenerateProjects2();
		}

		private void GenerateProjects1()
		{
			var teamsRefs = _teamsData.GeneratedItems
				.Select(n => new ObjectReference { Id = n.Id })
				.ToList();

			_projectsData
				.WithArray(20, Spec.Models.Generators.GetProjectGenerator())
				.WithArray(20, Spec.Models.Generators.GetProjectGenerator().WithRange(x => x.Team, teamsRefs))
				.Generate();
		}

		private void GenerateProjects2()
		{
			var parentRefs = _projectsData.GeneratedItems
				.Select(n => new ObjectReference { Id = n.Id })
				.ToList();

			var subprojectGenerator = new ArrayGenerator<ObjectReference>(new RangeGenerator<ObjectReference>(parentRefs))
			{
				Size = 4,
				Options = ArrayGenerationOptions.RandomSize
			};

			_projectsData
				.WithArray(10, Spec.Models.Generators.GetProjectGenerator().With(x => x.SubProjects, subprojectGenerator))
				.WithArray(10, Spec.Models.Generators.GetProjectGenerator()
					.With(x => x.SubProjects, subprojectGenerator)
					.WithStatic(x => x.Status, "Approved"))
				.Generate();
		}
	}
}