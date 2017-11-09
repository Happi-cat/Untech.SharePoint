using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Utils;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data;
using Untech.SharePoint.Data.Mapper;
using Untech.SharePoint.MetaModels;

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

			_spList = clientContext.GetListByUrl(list.Url);
		}

		public override IEnumerable<string> GetAttachments(int id)
		{
			var spItemAttachments = _spList.GetItemById(id).AttachmentFiles;

			_clientContext.Load(spItemAttachments);
			_clientContext.ExecuteQuery();
			var urlPrefix = new Uri(_clientContext.Url)
				.GetLeftPart(UriPartial.Authority);

			foreach (var attachment in spItemAttachments)
			{
				yield return urlPrefix + attachment.ServerRelativeUrl;
			}
		}

		protected override IEnumerable<ListItem> FetchInternal(string caml)
		{
			var listCollection = _spList.GetItems(CamlUtility.CamlStringToSPQuery(caml));

			_clientContext.Load(listCollection);
			_clientContext.ExecuteQuery();

			return listCollection.Cast<ListItem>();
		}

		protected override ListItem GetInternal(int id, MetaContentType contentType)
		{
			var spItem = _spList.GetItemById(id);
			_clientContext.Load(spItem, n => n, n => n.ContentType.StringId);
			_clientContext.ExecuteQuery();

			if (FilterByContentType && spItem.ContentType.StringId != contentType.Id)
			{
				throw new InvalidOperationException("ContentType mismatch");
			}

			return spItem;
		}

		protected override object AddInternal(object item, TypeMapper<ListItem> mapper)
		{
			var info = new ListItemCreationInformation();
			var spItem = _spList.AddItem(info);

			mapper.Map(item, spItem);

			spItem.Update();
			_clientContext.Load(spItem);
			_clientContext.ExecuteQuery();

			return mapper.CreateAndMap(spItem);
		}

		protected override void AddInternal(IEnumerable<object> items, TypeMapper<ListItem> mapper)
		{
			foreach (var item in items)
			{
				var info = new ListItemCreationInformation();
				var spItem = _spList.AddItem(info);

				mapper.Map(item, spItem);

				spItem.Update();
			}

			_clientContext.ExecuteQuery();
		}

		protected override object UpdateInternal(int id, object item, TypeMapper<ListItem> mapper)
		{
			var spItem = _spList.GetItemById(id);
			_clientContext.Load(spItem);
			_clientContext.ExecuteQuery();

			mapper.Map(item, spItem);

			spItem.Update();
			_clientContext.ExecuteQuery();

			return mapper.CreateAndMap(spItem);
		}

		protected override void UpdateInternal(IEnumerable<KeyValuePair<int, object>> items, TypeMapper<ListItem> mapper)
		{
			var loadedItems = new List<UpdateItemInfo>();

			foreach (var pair in items)
			{
				var spItem = _spList.GetItemById(pair.Key);
				_clientContext.Load(spItem);
				loadedItems.Add(new UpdateItemInfo(spItem, pair.Value));
			}
			_clientContext.ExecuteQuery();

			foreach (var info in loadedItems)
			{
				mapper.Map(info.Item, info.SpItem);

				info.SpItem.Update();
			}
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

		protected override void DeleteInternal(IEnumerable<int> ids)
		{
			var loadedItems = new List<ListItem>();
			foreach (var id in ids)
			{
				var spItem = _spList.GetItemById(id);
				_clientContext.Load(spItem);
				loadedItems.Add(spItem);
			}
			_clientContext.ExecuteQuery();

			foreach (var item in loadedItems)
			{
				item.DeleteObject();
			}
			_clientContext.ExecuteQuery();
		}

		private class UpdateItemInfo
		{
			public UpdateItemInfo([NotNull] ListItem spItem, [NotNull] object item)
			{
				SpItem = spItem;
				Item = item;
			}

			[NotNull]
			public ListItem SpItem { get; }

			[NotNull]
			public object Item { get; }
		}
	}
}