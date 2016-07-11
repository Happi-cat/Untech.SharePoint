using Untech.SharePoint.Common.Mappings.ClassLike;

namespace Untech.SharePoint.Common.Test.Mappings.ClassLike
{
	public class SmallDataContextMap : ContextMap<SmallDataContext>
	{
		public SmallDataContextMap()
		{
			List("/Lists/QuickLinks")
				.ContentType(n => n.QuickLinks, new QuickLinksMap());

			List("/Lists/NavigationMenu")
				.ContentType(n => n.NavigationMenu, new NavigationMenuMap());

			List("/Lists/News")
				.ContentType(n => n.Announcments, new AnnouncementsMap())
				.ContentType(n => n.Events, new EventsMap());

			List("/Lists/NavigationMenu")
				.AnnotatedContentType(n => n.Entities);
		}

		public class QuickLinksMap : ContentTypeMap<MenuItem>
		{
			public QuickLinksMap()
			{
				ContentTypeId("0x01");

				Field(n => n.Title).InternalName("Title");
				Field(n => n.Description).InternalName("ShortDescription");
				Field(n => n.Url).InternalName("Link").TypeAsString("Text");
			}
		}

		public class NavigationMenuMap : ContentTypeMap<MenuItem>
		{
			public NavigationMenuMap()
			{
				Field(n => n.Title).InternalName("Title");
				Field(n => n.Description).InternalName("Description");
				Field(n => n.Url).InternalName("Target").TypeAsString("Text");
			}
		}

		public class AnnouncementsMap : ContentTypeMap<AnnouncmentItem>
		{
			public AnnouncementsMap()
			{
				ContentTypeId("0x0101");

				Field(n => n.Title).InternalName("Title");
				Field(n => n.Body).InternalName("Content");
			}
		}

		public class EventsMap : ContentTypeMap<EventItem>
		{
			public EventsMap()
			{
				Field(n => n.Title).InternalName("Title");
				Field(n => n.Description).InternalName("Content");
				Field(n => n.MeetDate);
			}
		}
	}
}