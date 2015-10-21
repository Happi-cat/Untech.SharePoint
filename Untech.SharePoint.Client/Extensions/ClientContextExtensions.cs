using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Extensions
{
	internal static class ClientContextExtensions
	{
		public static List GetList(this ClientContext context, string listTitle)
		{
			var list = context.Web.Lists.GetByTitle(listTitle);
			context.Load(list, l => l, l => l.Fields, l=> l.ContentTypes);
			context.ExecuteQuery();

			return list;
		}
	}
}
