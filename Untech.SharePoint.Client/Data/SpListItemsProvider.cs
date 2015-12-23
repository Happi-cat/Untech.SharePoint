using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Utils;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.Data
{
	internal class SpListItemsProvider : BaseSpListItemsProvider<ListItem>
	{
		public SpListItemsProvider(ClientContext clientContext, SpCommonService commonService, MetaList list)
			: base(list)
		{
			ClientContext = clientContext;
			CommonService = commonService;

			SpList = clientContext.GetList(list.Title);
		}

		private ClientContext ClientContext { get; set; }

		private SpCommonService CommonService { get; set; }

		private List SpList { get; set; }

		protected override IList<ListItem> FetchInternal(string caml)
		{
			var listCollection = SpList.GetItems(CamlUtility.CamlStringToSPQuery(caml));

			ClientContext.Load(listCollection);
			ClientContext.ExecuteQuery();

			return listCollection.Cast<ListItem>().ToList();
		}

		protected override ListItem GetInternal(int id, MetaContentType contentType)
		{
			var spItem = SpList.GetItemById(id);
			ClientContext.Load(spItem, n => n, n => n.ContentType.StringId);
			ClientContext.ExecuteQuery();

			if (spItem.ContentType.StringId != contentType.Id)
			{
				throw new InvalidOperationException("ContentType mismatch");
			}

			return spItem;
		}

		protected override int AddInternal(object item, MetaContentType contentType)
		{
			var mapper = contentType.GetMapper<ListItem>();

			var info = new ListItemCreationInformation();
			var spItem = SpList.AddItem(info);

			mapper.Map(item, spItem);

			spItem.Update();
			ClientContext.ExecuteQuery();

			return spItem.Id;
		}

		protected override void UpdateInternal(int id, object item, MetaContentType contentType)
		{
			var mapper = contentType.GetMapper<ListItem>();

			var spItem = SpList.GetItemById(id);
			ClientContext.Load(spItem);
			ClientContext.ExecuteQuery();

			mapper.Map(item, spItem);

			spItem.Update();
			ClientContext.ExecuteQuery();
		}

		protected override void DeleteInternal(int id, MetaContentType contentType)
		{
			var spItem = SpList.GetItemById(id);
			ClientContext.Load(spItem);
			ClientContext.ExecuteQuery();

			spItem.DeleteObject();
			ClientContext.ExecuteQuery();
		}
	}
}