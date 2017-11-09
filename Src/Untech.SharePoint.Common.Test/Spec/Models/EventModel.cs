using System;
using Untech.SharePoint.Mappings.Annotation;
using Untech.SharePoint.Models;

namespace Untech.SharePoint.Spec.Models
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