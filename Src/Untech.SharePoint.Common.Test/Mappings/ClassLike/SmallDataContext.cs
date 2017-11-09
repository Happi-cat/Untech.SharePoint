using Untech.SharePoint.Data;
using Untech.SharePoint.Models;

namespace Untech.SharePoint.Mappings.ClassLike
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