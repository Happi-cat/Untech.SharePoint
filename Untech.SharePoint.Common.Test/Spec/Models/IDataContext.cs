using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Spec.Models
{
	public interface IDataContext : ISpContext
	{
		[SpList(Title = "News")]
		ISpList<NewsModel> News { get; }

		[SpList(Title = "Events")]
		ISpList<EventModel> Events { get; }

		[SpList(Title = "Teams")]
		ISpList<TeamModel> Teams { get; }

		[SpList(Title = "Projects")]
		ISpList<ProjectModel> Projects { get; }
	}
}