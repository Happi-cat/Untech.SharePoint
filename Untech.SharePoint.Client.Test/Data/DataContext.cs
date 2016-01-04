using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Test.Spec.Models;

namespace Untech.SharePoint.Client.Test.Data
{
	public class DataContext : SpClientContext<DataContext>, IDataContext
	{
		public DataContext(ClientContext context, Config config) 
			: base(context, config)
		{

		}

		[SpList]
		public ISpList<NewsModel> News
		{
			get { return GetList(x => x.News); }
		}

		[SpList]
		public ISpList<EventModel> Events
		{
			get { return GetList(x => x.Events); }
		}

		[SpList]
		public ISpList<TeamModel> Teams
		{
			get { return GetList(x => x.Teams); }
		}

		[SpList]
		public ISpList<ProjectModel> Projects
		{
			get { return GetList(x => x.Projects); }
		}

	}
}