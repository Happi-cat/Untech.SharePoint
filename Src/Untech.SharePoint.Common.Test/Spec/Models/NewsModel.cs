using Untech.SharePoint.Mappings.Annotation;
using Untech.SharePoint.Models;

namespace Untech.SharePoint.Spec.Models
{
	[SpContentType]
	public class NewsModel : Entity
	{
		[SpField]
		public string Body { get; set; }

		[SpField]
		public string Description { get; set; }

		[SpField(Name = "HeadingImage")]
		public string HeadingImageUrl { get; set; }

		[SpField(Name = "HeadingImage")]
		public UrlInfo HeadingImage { get; set; }
	}
}