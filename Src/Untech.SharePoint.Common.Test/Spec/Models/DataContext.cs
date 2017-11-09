using Untech.SharePoint.Data;
using Untech.SharePoint.Mappings.Annotation;

namespace Untech.SharePoint.Spec.Models
{
	public class DataContext : SpContext<DataContext>
	{
		public DataContext(ICommonService commonService)
			: base(commonService)
		{
		}

		[SpList("/Lists/News")]
		public ISpList<NewsModel> News
		{
			get { return GetList(x => x.News); }
		}

		[SpList("/Lists/Events")]
		public ISpList<EventModel> Events
		{
			get { return GetList(x => x.Events); }
		}

		[SpList("/Lists/Teams")]
		public ISpList<TeamModel> Teams
		{
			get { return GetList(x => x.Teams); }
		}

		[SpList("/Lists/Projects")]
		public ISpList<ProjectModel> Projects
		{
			get { return GetList(x => x.Projects); }
		}
	}
}