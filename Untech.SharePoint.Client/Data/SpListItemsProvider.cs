using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Utils;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.Data
{
	internal class SpListItemsProvider : BaseSpListItemsProvider<ListItem>
	{
		private readonly ClientContext _clientContext;

		private readonly List _spList;

		public SpListItemsProvider(ClientContext clientContext, MetaList list)
			: base(list)
		{
			_clientContext = clientContext;

			_spList = clientContext.GetList(list.Title);
		}


		protected override IList<ListItem> FetchInternal(string caml)
		{
			var listCollection = _spList.GetItems(CamlUtility.CamlStringToSPQuery(caml));

			_clientContext.Load(listCollection);
			_clientContext.ExecuteQuery();

			return listCollection.Cast<ListItem>().ToList();
		}

		protected override ListItem GetInternal(int id, MetaContentType contentType)
		{
			var spItem = _spList.GetItemById(id);
			_clientContext.Load(spItem, n => n, n => n.ContentType.StringId);
			_clientContext.ExecuteQuery();

			if (spItem.ContentType.StringId != contentType.Id)
			{
				throw new InvalidOperationException("ContentType mismatch");
			}

			return spItem;
		}

		protected override int AddInternal(object item, TypeMapper<ListItem> mapper)
		{
			var info = new ListItemCreationInformation();
			var spItem = _spList.AddItem(info);

			mapper.Map(item, spItem);

			spItem.Update();
			_clientContext.ExecuteQuery();

			return spItem.Id;
		}

		protected override void UpdateInternal(int id, object item, TypeMapper<ListItem> mapper)
		{
			var spItem = _spList.GetItemById(id);
			_clientContext.Load(spItem);
			_clientContext.ExecuteQuery();

			mapper.Map(item, spItem);

			spItem.Update();
			_clientContext.ExecuteQuery();
		}

		protected override void DeleteInternal(int id)
		{
			var spItem = _spList.GetItemById(id);
			_clientContext.Load(spItem);
			_clientContext.ExecuteQuery();

			spItem.DeleteObject();
			_clientContext.ExecuteQuery();
		}
	}
}