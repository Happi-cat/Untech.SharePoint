using System;
using System.Runtime.Serialization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Converters.Custom;
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

	[JsonConverter(typeof(StringEnumConverter))]
	public enum ProjectStatus
	{
		Invalid = 0,
		Draft,
		[EnumMember(Value = "Pending Decision")]
		Pending,
		Approved,
		Rejected,
		Rework,
		Cancelled,
		Completed,
		[EnumMember(Value = "On Hold")]
		OnHold
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum ProjectGate
	{
		Invalid = 0,
		[EnumMember(Value = "Business Proposal")]
		Idea,
		Initiation,
		Commit,
		Development,
		Testing,
		Launch,
		Realization
	}

	[SpContentType(Id = "0x0100ED4D646C7EDE473BA41EC3DD8473CFFF")]
	public class BaseProject : Entity
	{
		[SpField(Name = "BIFGate", CustomConverterType = typeof(EnumFieldConverter))]
		public virtual ProjectGate Gate { get; set; }

		[SpField(Name = "BIFProjectUid")]
		public virtual string ProjectUid { get; set; }

		[SpField(Name = "BIFProjectNo")]
		public virtual int? ProjectNo { get; set; }

		[SpField(Name = "BIFLobBudgetOwner")]
		public virtual UserInfo LobBudgetOwner { get; set; }

		[SpField(Name = "BIFLobDeliveryOwner")]
		public virtual UserInfo LobDeliveryOwner { get; set; }

		[SpField(Name = "BIFProjectSponsoringPortfolio")]
		public virtual string SponsoringPortfolio { get; set; }

		[SpField(Name = "BIFProjectParentPortfolio")]
		public virtual string ParentPortfolio { get; set; }

		[SpField(Name = "BIFProjectChildPortfolio")]
		public virtual string ChildPortfolio { get; set; }

		[SpField(Name = "BIFProjectMethodology")]
		public virtual string Methodology { get; set; }

		[SpField(Name = "BIFStatus", CustomConverterType = typeof(EnumFieldConverter))]
		public virtual ProjectStatus Status { get; set; }

		[SpField(Name = "BIFApprover")]
		public virtual UserInfo Approver { get; set; }

		[SpField(Name = "BIFDecisionDate")]
		public virtual DateTime? DecisionDate { get; set; }

		[SpField(Name = "BIFProjectComments")]
		public virtual string ProjectComments { get; set; }
	}

	[SpContentType(Id = "0x0100ED4D646C7EDE473BA41EC3DD8473CFFF01")]
	public class DevelopmentProject : BaseProject
	{
	}

	[SpContentType(Id = "0x0100ED4D646C7EDE473BA41EC3DD8473CFFF02")]
	public class MpsProject : BaseProject
	{
	}

	[SpContentType(Id = "0x0100ED4D646C7EDE473BA41EC3DD8473CFFF03")]
	public class NplProject : BaseProject
	{
	}
}