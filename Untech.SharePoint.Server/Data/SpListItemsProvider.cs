using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.Mapper;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Server.Utils;

namespace Untech.SharePoint.Server.Data
{
	internal class SpListItemsProvider : BaseSpListItemsProvider<SPListItem>
	{
		private readonly SPWeb _spWeb;
		private readonly SPList _spList;

		public SpListItemsProvider(SPWeb web, MetaList list)
			: base(list)
		{
			_spWeb = web;
			_spList = web.Lists[list.Title];
		}

		protected override IList<SPListItem> FetchInternal(string caml)
		{
			return _spList.GetItems(CamlUtility.CamlStringToSPQuery(caml))
				.Cast<SPListItem>()
				.ToList();
		}

		protected override SPListItem GetInternal(int id, MetaContentType contentType)
		{
			var spItem = _spList.GetItemById(id);

			if (spItem.ContentTypeId.ToString() != contentType.Id)
			{
				throw new InvalidOperationException("ContentType mismatch");
			}

			return spItem;
		}

		protected override object AddInternal(object item, TypeMapper<SPListItem> mapper)
		{
			var spItem = _spList.AddItem();

			mapper.Map(item, spItem);

			spItem.Update();

			return mapper.CreateAndMap(spItem);
		}

		protected override void AddInternal(IEnumerable<object> items, TypeMapper<SPListItem> mapper)
		{
			var batchBuilder = new BatchBuilder();
			batchBuilder.Begin();

			foreach (var item in items)
			{
				batchBuilder.NewItem(_spList,  mapper.MapToCaml(item));
			}

			batchBuilder.End();

			_spWeb.ProcessBatchData(batchBuilder.ToString());
		}

		protected override object UpdateInternal(int id, object item, TypeMapper<SPListItem> mapper)
		{
			var spItem = _spList.GetItemById(id);

			mapper.Map(item, spItem);

			spItem.Update();

			return mapper.CreateAndMap(spItem);
		}

		protected override void UpdateInternal(IEnumerable<KeyValuePair<int, object>> items, TypeMapper<SPListItem> mapper)
		{
			var batchBuilder = new BatchBuilder();
			batchBuilder.Begin();

			foreach (var pair in items)
			{
				batchBuilder.UpdateItem(_spList, mapper.MapToCaml(pair.Value));
			}

			batchBuilder.End();

			_spWeb.ProcessBatchData(batchBuilder.ToString());
		}

		protected override void DeleteInternal(int id)
		{
			var spItem = _spList.GetItemById(id);

			spItem.Delete();
		}

		protected override void DeleteInternal(IEnumerable<int> ids)
		{
			var batchBuilder = new BatchBuilder();
			batchBuilder.Begin();

			foreach (var id in ids)
			{
				batchBuilder.DeleteItem(_spList, id.ToString());
			}

			batchBuilder.End();

			_spWeb.ProcessBatchData(batchBuilder.ToString());
		}
	}
}