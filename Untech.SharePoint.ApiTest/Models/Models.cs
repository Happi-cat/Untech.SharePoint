using System;
using System.Collections.Generic;
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

		[SpList(Title = "News")]
		public ISpList<NewsItem> News { get { return GetList(x => x.News); } }

		[SpList(Title = "Events")]
		public ISpList<EventItem> Events { get { return GetList(x => x.Events); } }

		[SpList(Title = "Teams")]
		public ISpList<TeamItem> Teams { get { return GetList(x => x.Teams); } }

		[SpList(Title = "Projects")]
		public ISpList<ProjectItem> Projects { get { return GetList(x => x.Projects); } }
	}

	public class ClientDataContext : SpClientContext<ClientDataContext>
	{
		public ClientDataContext(ClientContext context, Config config)
			: base(context, config)
		{
		}

		[SpList(Title = "News")]
		public ISpList<NewsItem> News { get { return GetList(x => x.News); } }

		[SpList(Title = "Events")]
		public ISpList<EventItem> Events { get { return GetList(x => x.Events); } }

		[SpList(Title = "Teams")]
		public ISpList<TeamItem> Teams { get { return GetList(x => x.Teams); } }

		[SpList(Title = "Projects")]
		public ISpList<ProjectItem> Projects { get { return GetList(x => x.Projects); } }
	}


	[SpContentType]
	public class NewsItem : Entity
	{
		[SpField]
		public string Body { get; set; }

		[SpField]
		public string Description { get; set; }

		[SpField(Name = "HeadingImage")]
		public string HeadingImageUrl { get; set; }

		[SpField(Name = "HeadingImage")]
		public UrlInfo HeadingImage { get; set; }
	}

	[SpContentType]
	public class EventItem : Entity
	{
		[SpField]
		public DateTime? WhenStart { get; set; }

		[SpField]
		public DateTime? WhenComplete { get; set; }

		[SpField]
		public double? MaxPeople { get; set; }

		[SpField]
		public UserInfo Organizer { get; set; }

		[SpField]
		public UrlInfo Logo { get; set; }

		[SpField]
		public bool? FreeEntrance { get; set; }
	}

	[SpContentType]
	public class TeamItem : Entity
	{
		[SpField]
		public UserInfo Manager { get; set; }

		public List<UserInfo> Developers { get; set; }

		[SpField]
		public UrlInfo Logo { get; set; }
	}

	[SpContentType]
	public class ProjectItem : Entity
	{
		[SpField]
		public ObjectReference Team { get; set; }


		[SpField]
		public DateTime? ProjectStart { get; set; }

		[SpField]
		public DateTime? ProjectEnd { get; set; }

		[SpField(Name ="OS")]
		public string[] OSes { get; set; }

		[SpField]
		public string Technology { get; set; }

		[SpField]
		public List<ObjectReference> SubProjects { get; set; }

		[SpField]
		public double? Duration { get; set; }

		[SpField]
		public string Status { get; set; }

		[SpField]
		public UserInfo FinanceManager { get; set; }
	}
}