using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Models
{
	public class UrlInfo
	{
		public UrlInfo()
		{
			
		}

		internal UrlInfo(SPFieldUrlValue value)
		{
			Url = value.Url;
			Title = value.Description;
		}

		public string Url { get; set; }
		public string Title { get; set; }
	}
}