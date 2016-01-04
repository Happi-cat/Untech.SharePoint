﻿using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Common.Test.Tools.Generators;
using Untech.SharePoint.Common.Test.Tools.Generators.Basic;
using Untech.SharePoint.Common.Test.Tools.Generators.Custom;

namespace Untech.SharePoint.Common.Test.Spec.Models
{
	public static class Generators
	{
		public static ObjectGenerator<EventModel> GetCompletedEventGenerator()
		{
			return GetBaseEventGenerator()
				.WithPastDate(n => n.WhenStart)
				.WithPastDate(n => n.WhenComplete);
		}

		public static ObjectGenerator<EventModel> GetGoingEventGenerator()
		{
			return GetBaseEventGenerator()
				.WithActualDate(n => n.WhenStart)
				.WithActualDate(n => n.WhenComplete);
		}

		public static ObjectGenerator<EventModel> GetFutureEventGenerator()
		{
			return GetBaseEventGenerator()
				.WithFutureDate(n => n.WhenStart)
				.WithFutureDate(n => n.WhenComplete);
		}

		public static ObjectGenerator<NewsModel> GetNewsGenerator()
		{
			return GetBaseEntityGenerator<NewsModel>()
				.WithHtmlLongLorem(n => n.Body)
				.WithLongLorem(n => n.Description)
				.With(n => n.HeadingImage, new UrlGenerator());
		}

		public static ObjectGenerator<TeamModel> GetTeamGenerator()
		{
			return GetBaseEntityGenerator<TeamModel>()
				.With(n => n.Logo, new UrlGenerator());
		}

		public static ObjectGenerator<ProjectModel> GetProjectGenerator()
		{
			return GetBaseEntityGenerator<ProjectModel>()
				.WithActualDate(n => n.ProjectStart)
				.WithActualDate(n => n.ProjectEnd)
				.WithArray(n => n.OSes, 3, new[] {"Linux", "MaxOS", "Windows"})
				.WithRange(n => n.Status, new[] {"Approved"})
				.WithRange(n => n.Technology, new[] {".NET", "NodeJS", "Java"});
		}

		private static ObjectGenerator<EventModel> GetBaseEventGenerator()
		{
			return GetBaseEntityGenerator<EventModel>()
				.With(n => n.Logo, new UrlGenerator())
				.With(n => n.MaxPeople, new DoubleRangeGenerator { Min = 1, Max = 10 })
				.With(n => n.FreeEntrance, new BoolGenerator());
		}

		private static ObjectGenerator<T> GetBaseEntityGenerator<T>()
			where T: Entity
		{
			return new ObjectGenerator<T>()
				.WithShortLorem(n => n.Title);
		}

	}
}