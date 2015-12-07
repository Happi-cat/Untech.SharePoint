using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.ApiTest.Models
{
	public class ServerDataContext : SpServerContext<ServerDataContext>
	{
		public ServerDataContext(SPWeb web, Config config) : base(web, config)
		{
		}

		[SpList(Title = "Projects Register")]
		public ISpList<DevelopmentProject> DevelopmentProjects { get { return GetList(x => x.DevelopmentProjects); } }

		[SpList(Title = "Projects Register")]
		public ISpList<MpsProject> MpsProjects{ get { return GetList(x => x.MpsProjects); } }

		[SpList(Title = "Projects Register")]
		public ISpList<NplProject> NplProjects { get { return GetList(x => x.NplProjects); } }
	}

	public class ClientDataContext : SpClientContext<ClientDataContext>
	{
		public ClientDataContext(ClientContext context, Config config)
			: base(context, config)
		{
		}

		[SpList(Title = "Projects Register")]
		public ISpList<DevelopmentProject> DevelopmentProjects { get { return GetList(x => x.DevelopmentProjects); } }

		[SpList(Title = "Projects Register")]
		public ISpList<MpsProject> MpsProjects { get { return GetList(x => x.MpsProjects); } }

		[SpList(Title = "Projects Register")]
		public ISpList<NplProject> NplProjects { get { return GetList(x => x.NplProjects); } }
	}

	public class Project : Entity
	{
		 
	}

	public class DevelopmentProject : Project
	{

	}

	public class MpsProject : Project
	{

	}

	public class NplProject : Project
	{

	}
}