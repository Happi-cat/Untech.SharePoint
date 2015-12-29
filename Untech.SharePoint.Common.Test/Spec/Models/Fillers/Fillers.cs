using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Tools.Generators;

namespace Untech.SharePoint.Common.Test.Spec.Models.Fillers
{
	public static class Fillers
	{
		public static ItemFiller<EventModel> GetCompletedEventFiller()
		{
			return GetBaseEventFiller()
				.WithPastDate(n => n.WhenStart)
				.WithPastDate(n => n.WhenComplete);
		}

		public static ItemFiller<EventModel> GetGoingEventFiller()
		{
			return GetBaseEventFiller()
				.WithActualDate(n => n.WhenStart)
				.WithActualDate(n => n.WhenComplete);
		}

		public static ItemFiller<EventModel> GetFutureEventFiller()
		{
			return GetBaseEventFiller()
				.WithFutureDate(n => n.WhenStart)
				.WithFutureDate(n => n.WhenComplete);
		}

		public static ItemFiller<NewsModel> GetNewsFiller()
		{
			return GetBaseEntityFiller<NewsModel>()
				.WithHtmlLongLorem(n => n.Body)
				.WithLongLorem(n => n.Description)
				.With(n => n.HeadingImage, new UrlGenerator());
		}

		public static ItemFiller<TeamModel> GetTeamFiller()
		{
			return GetBaseEntityFiller<TeamModel>()
				.With(n => n.Logo, new UrlGenerator());
		}

		public static ItemFiller<ProjectModel> GetProjectFiller()
		{
			return GetBaseEntityFiller<ProjectModel>()
				.WithActualDate(n => n.ProjectStart)
				.WithActualDate(n => n.ProjectEnd)
				.WithArray(n => n.OSes, 3, new[] {"Linux", "MaxOS", "Windows"})
				.WithRange(n => n.Status, new[] {"Approved"})
				.WithRange(n => n.Technology, new[] {".NET", "NodeJS", "Java"});
		}

		private static ItemFiller<EventModel> GetBaseEventFiller()
		{
			return GetBaseEntityFiller<EventModel>()
				.With(n => n.Logo, new UrlGenerator())
				.With(n => n.MaxPeople, new DoubleRangeGenerator { Min = 1, Max = 10 })
				.With(n => n.FreeEntrance, new BoolGenerator());
		}

		private static ItemFiller<T> GetBaseEntityFiller<T>()
			where T: Entity
		{
			return new ItemFiller<T>()
				.WithShortLorem(n => n.Title);
		}

	}
}