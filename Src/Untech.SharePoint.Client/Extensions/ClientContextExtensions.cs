using System;
using System.IO;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Extensions
{
	internal static class ClientContextExtensions
	{
		/// <summary>
		/// Returns <see cref="List"/> by server-relative list URL.
		/// </summary>
		/// <param name="context">Current <see cref="ClientContext"/>.</param>
		/// <param name="listUrl">The site-relative list URL.</param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException">List cannot be found by <paramref name="listUrl"/></exception>
		public static List GetListByUrl(this ClientContext context, string listUrl)
		{
			var serverRelativeUrl = context.Url.TrimEnd('/') + "/" + listUrl.TrimStart('/');

			try
			{
				var list = context.Web.GetList(serverRelativeUrl);
				context.Load(list, l => l, l => l.Fields, l => l.ContentTypes);
				context.ExecuteQuery();

				return list;
			}
			catch (Exception e)
			{
				throw new FileNotFoundException($"Unable to find or load list by url: ${listUrl}", e);
			}
		}
	}
}
