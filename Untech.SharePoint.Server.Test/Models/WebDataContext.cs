using Microsoft.SharePoint;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Server.Data;

namespace Untech.SharePoint.Server.Test.Models
{
	public class WebDataContext : SpServerContext<WebDataContext>
	{
		public WebDataContext(SPWeb web, Config config) 
			: base(web, config)
		{

		}

		[SpList(Title = "News")]
		public ISpList<NewsItem> News { get { return GetList(x => x.News); }}
	}
}