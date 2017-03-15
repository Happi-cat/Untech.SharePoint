using System;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.Spec.Models
{
	[SpContentType]
	public class EventModel : Entity
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
}