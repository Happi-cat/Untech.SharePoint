using System.Collections.Generic;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.Test.Spec.Models
{
	[SpContentType]
	public class TeamModel : Entity
	{
		[SpField]
		public UserInfo Manager { get; set; }

		public List<UserInfo> Developers { get; set; }

		[SpField]
		public UrlInfo Logo { get; set; }
	}
}