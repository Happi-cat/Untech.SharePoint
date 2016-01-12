using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.Spec.Models
{
	public interface IDataContext : ISpContext
	{
		ISpList<NewsModel> News { get; }

		ISpList<EventModel> Events { get; }

		ISpList<TeamModel> Teams { get; }

		ISpList<ProjectModel> Projects { get; }
	}
}