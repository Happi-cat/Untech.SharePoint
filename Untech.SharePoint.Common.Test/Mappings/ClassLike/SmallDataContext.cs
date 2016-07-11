using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Models;

namespace Untech.SharePoint.Common.Test.Mappings.ClassLike
{
	public class SmallDataContext : SpContext<SmallDataContext>
	{
		public SmallDataContext(ICommonService commonService)
			: base(commonService)
		{
		}

		public ISpList<MenuItem> QuickLinks { get; set; }

		public ISpList<MenuItem> NavigationMenu { get; set; }

		public ISpList<AnnouncmentItem> Announcments { get; set; }

		public ISpList<EventItem> Events { get; set; }

		public ISpList<Entity> Entities { get; set; }
	}

}