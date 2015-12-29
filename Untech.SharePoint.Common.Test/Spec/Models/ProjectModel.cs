using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.Test.Spec.Models
{
	[SpContentType]
	public class ProjectModel : Entity
	{
		[SpField]
		public ObjectReference Team { get; set; }


		[SpField]
		public DateTime? ProjectStart { get; set; }

		[SpField]
		public DateTime? ProjectEnd { get; set; }

		[SpField(Name = "OS")]
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