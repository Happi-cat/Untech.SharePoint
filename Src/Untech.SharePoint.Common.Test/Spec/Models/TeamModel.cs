using System.Collections.Generic;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.Spec.Models
{
	[SpContentType]
	public class TeamModel : Entity
	{
		[SpField]
		public UserInfo ProjectManager { get; set; }

		[SpField]
		public UserInfo FinanceManager { get; set; }

		[SpField]
		public UserInfo BusinessAnalyst { get; set; }

		[SpField]
		public UserInfo SoftwareArchitect { get; set; }

		[SpField]
		public UserInfo DatabaseArchitect { get; set; }

		[SpField]
		public List<UserInfo> BackendDevelopers { get; set; }

		[SpField]
		public List<UserInfo> FrontendDevelopers { get; set; }

		[SpField]
		public List<UserInfo> Designers { get; set; }

		[SpField]
		public List<UserInfo> Testers { get; set; }

		[SpField]
		public UrlInfo Logo { get; set; }
	}
}