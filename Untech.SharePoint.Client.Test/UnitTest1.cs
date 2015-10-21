using System.Linq;
using System.Net;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Client.Configuration;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Client.Test
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

	public class InvestmenFrameworkContext : SpClientContext<InvestmenFrameworkContext>
	{
		public InvestmenFrameworkContext(ClientContext context, Config config)
			: base(context, config)
		{
		}

		[SpList(Title = "Projects Register")]
		public ISpList<InvestmentProject> InvestmentProjects { get { return GetList(context => context.InvestmentProjects); } }
	}


	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var cfg = ClientConfig.Begin().RegisterMappings(n => n.Annotated<InvestmenFrameworkContext>()).BuildConfig();

			var clientCtx = new ClientContext("http://spnthpdvds0040v:8086/sites/investment");
			var ctx = new InvestmenFrameworkContext(clientCtx, cfg);

			var result = ctx.InvestmentProjects.Where(n => n.ProjectUniqueId.StartsWith("TTT") && n.Status == "Approved").ToList();
		}
	}
}
