using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Client.Test.Models
{
	public class WebDataContext : SpClientContext<WebDataContext>
	{
		public WebDataContext(ClientContext context, Config config)
			: base(context, config)
		{
		}

		[SpList(Title = "News")]
		public ISpList<NewsItem> News { get { return GetList(x => x.News); }}
	}
}