using System;
using System.IO;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Server.Extensions
{
	internal static class SPWebExtensions
	{
		/// <summary>
		/// Returns <see cref="SPList"/> by server-relative list URL.
		/// </summary>
		/// <param name="web">Current <see cref="SPWeb"/>.</param>
		/// <param name="listUrl">The site-relative list URL.</param>
		/// <returns></returns>
		public static SPList GetListByUrl([NotNull] this SPWeb web, [NotNull] string listUrl)
		{
			var serverRelativeUrl = web.ServerRelativeUrl.TrimEnd('/') + "/" + listUrl.TrimStart('/');

			return web.GetList(serverRelativeUrl);
		}
	}
}
