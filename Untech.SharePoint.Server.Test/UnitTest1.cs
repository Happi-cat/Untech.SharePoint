using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Models;
using Untech.SharePoint.Server.Configuration;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Test
{
	public class InvestmentProject : Entity
	{
		[SpField(Name = "BIFGate")]
		public string Gate { get; set; }

		[SpField(Name = "BIFProjectUid")]
		public string ProjectUniqueId { get; set; }

		[SpField(Name = "BIFProjectNo")]
		public int? ProjectNo { get; set; }

		[SpField(Name = "BIFProjectSponsoringPortfolio")]
		public string SponsoringPortfolio { get; set; }

		[SpField(Name = "BIFProjectParentPortfolio")]
		public string ParentPortfolio { get; set; }

		[SpField(Name = "BIFProjectChildPortfolio")]
		public string ChildPortfolio { get; set; }

		[SpField(Name = "BIFProjectMethodology")]
		public string Methodology { get; set; }

		[SpField(Name = "BIFStatus")]
		public string Status { get; set; }
	}

	public class InvestmenFrameworkContext : SpServerContext<InvestmenFrameworkContext>
	{
		public InvestmenFrameworkContext(SPWeb web, Config config) : base(web, config)
		{
		}

		[SpList(Title = "Projects Register")]
		public ISpList<InvestmentProject>  InvestmentProjects {get { return GetList(context => context.InvestmentProjects); }}
	}


	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var cfg = ServerConfig.Begin().RegisterMappings(n => n.Annotated<InvestmenFrameworkContext>()).BuildConfig();

			var ctx = new InvestmenFrameworkContext(new SPSite("jhttp://localhost:8086/sites/investment").OpenWeb(), cfg);

			var result = ctx.InvestmentProjects.Where(n => n.ProjectUniqueId.StartsWith("TTT") && n.Status == "Approved").ToList();
		}
	}
}
