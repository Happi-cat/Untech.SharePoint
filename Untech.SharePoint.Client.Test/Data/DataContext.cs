using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Client.Test.Data
{
	public class DataContext : SpClientContext<DataContext>, IDataContext
	{
		public DataContext(ClientContext context, Config config) 
			: base(context, config)
		{

		}

		public ISpList<NewsModel> News
		{
			get { return GetList(x => x.News); }
		}

		public ISpList<EventModel> Events
		{
			get { return GetList(x => x.Events); }
		}

		public ISpList<TeamModel> Teams
		{
			get { return GetList(x => x.Teams); }
		}

		public ISpList<ProjectModel> Projects
		{
			get { return GetList(x => x.Projects); }
		}

	}
}