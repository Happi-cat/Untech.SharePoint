using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data
{
	internal static class ClientContextExtensions
	{
		public static IEnumerable<Field> GetListFields(this ClientContext context, string listTitle)
		{
			List list = context.Web.Lists.GetByTitle(listTitle);

			context.Load(list.Fields);
			context.ExecuteQuery();

			return list.Fields.ToList();
		}

		public static IEnumerable<Field> GetListFields(this ClientContext context, Guid listId)
		{
			List list = context.Web.Lists.GetById(listId);

			context.Load(list.Fields);
			context.ExecuteQuery();

			return list.Fields.ToList();
		}

		public static string GetListTitle(this ClientContext context, Guid listId)
		{
			List list = context.Web.Lists.GetById(listId);

			context.Load(list, l => l.Title);
			context.ExecuteQuery();

			return list.Title;
		}
	}
}
