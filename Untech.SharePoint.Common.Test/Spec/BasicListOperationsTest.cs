//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Untech.SharePoint.Common.Test.Spec.Models;
//using Untech.SharePoint.Common.Test.Spec.Scenarios;
//using Untech.SharePoint.Common.Test.Tools;

//namespace Untech.SharePoint.Common.Test.Spec
//{
//	public class BasicListOperationsTest
//	{
//		private readonly IDataContext _dataContext;
//		private readonly ScenarioRunner _runner;

//		public BasicListOperationsTest(IDataContext context)
//		{
//			_runner = new ScenarioRunner();
//			_dataContext = context;
//		}

//		public BasicListOperationsTest(IDataContext context, ScenarioRunner runner)
//		{
//			_runner = runner;
//			_dataContext = context;
//		}

//		[TestMethod]
//		public void Add()
//		{
//			_runner.Run(GetType(), "Add", new IScenario []
//			{
//				new AddScenario<NewsModel>(_dataContext.News, Generators.GetNewsFiller()),
//				new AddScenario<EventModel>(_dataContext.Events, Generators.GetGoingEventFiller()),
//				new AddScenario<TeamModel>(_dataContext.Teams, Generators.GetTeamFiller()),
//				new AddScenario<ProjectModel>(_dataContext.Projects, Generators.GetProjectFiller())
//			});
//		}

//		[TestMethod]
//		public void Delete()
//		{
//			_runner.Run(GetType(), "Delete", new IScenario[]
//			{
//				new DeleteScenario<NewsModel>(_dataContext.News, Generators.GetNewsFiller()),
//				new DeleteScenario<EventModel>(_dataContext.Events, Generators.GetGoingEventFiller()),
//				new DeleteScenario<TeamModel>(_dataContext.Teams, Generators.GetTeamFiller()),
//				new DeleteScenario<ProjectModel>(_dataContext.Projects, Generators.GetProjectFiller())
//			});
//		}


//		[TestMethod]
//		public void Update()
//		{
//			_runner.Run(GetType(), "Update", new IScenario[]
//			{
//				new UpdateScenario<NewsModel>(_dataContext.News, Generators.GetNewsFiller()),
//				new UpdateScenario<EventModel>(_dataContext.Events, Generators.GetGoingEventFiller()),
//				new UpdateScenario<TeamModel>(_dataContext.Teams, Generators.GetTeamFiller()),
//				new UpdateScenario<ProjectModel>(_dataContext.Projects, Generators.GetProjectFiller())
//			});
//		}

//		[TestMethod]
//		public void Get()
//		{
//			_runner.Run(GetType(), "Get", new IScenario[]
//			{
//				new GetScenario<NewsModel>(_dataContext.News, Generators.GetNewsFiller()),
//				new GetScenario<EventModel>(_dataContext.Events, Generators.GetGoingEventFiller()),
//				new GetScenario<TeamModel>(_dataContext.Teams, Generators.GetTeamFiller()),
//				new GetScenario<ProjectModel>(_dataContext.Projects, Generators.GetProjectFiller())
//			});
//		}
//	}
//}
