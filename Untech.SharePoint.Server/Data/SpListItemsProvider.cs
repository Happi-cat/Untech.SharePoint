using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Utils;

namespace Untech.SharePoint.Server.Data
{
	internal class SpListItemsProvider : BaseSpListItemsProvider, ISpListItemsProvider
	{
		public SpListItemsProvider(SPWeb web, SpCommonService commonService, MetaList list)
			: base(list)
		{
			Web = web;
			CommonService = commonService;

			SpList = web.Lists[list.Title];
		}

		public SPWeb Web { get; private set; }

		public SpCommonService CommonService { get; private set; }

		public SPList SpList { get; private set; }

		public IEnumerable<T> Fetch<T>(QueryModel caml)
		{
			var viewFields = GetAndRewriteViewFields<T>(caml);

			return FetchInternal<T>(caml)
				.Select(n => Materialize<T>(n, viewFields));
		}

		public bool Any<T>(QueryModel caml)
		{
			return FetchInternal<T>(caml).Any();
		}

		public int Count<T>(QueryModel caml)
		{
			return FetchInternal<T>(caml).Count;
		}

		public T SingleOrDefault<T>(QueryModel caml)
		{
			var viewFields = GetAndRewriteViewFields<T>(caml);

			caml.RowLimit = 2;

			var foundItems = FetchInternal<T>(caml);

			if (foundItems.Count > 1)
			{
				throw Error.MoreThanOneMatch();
			}
			return foundItems.Count == 1 ? Materialize<T>(foundItems[0], viewFields) : default(T);
		}

		public T FirstOrDefault<T>(QueryModel caml)
		{
			var viewFields = GetAndRewriteViewFields<T>(caml);

			caml.RowLimit = 1;

			var foundItems = FetchInternal<T>(caml);

			return foundItems.Count == 1 ? Materialize<T>(foundItems[0], viewFields) : default(T);
		}

		public T ElementAtOrDefault<T>(QueryModel caml, int index)
		{
			var viewFields = GetAndRewriteViewFields<T>(caml);

			caml.RowLimit = index + 1;

			var foundItem = FetchInternal<T>(caml).ElementAtOrDefault(index);

			return foundItem != null ? Materialize<T>(foundItem, viewFields) : default(T);
		}

		public T Get<T>(int id)
		{
			if (List.IsExternal)
			{
				throw DataError.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var spItem = SpList.GetItemById(id);

			if (spItem.ContentTypeId.ToString() != contentType.Id)
			{
				throw new InvalidOperationException("ContentType mismatch");
			}

			return Materialize<T>(spItem);
		}

		public void Add<T>(T item)
		{
			if (List.IsExternal)
			{
				throw DataError.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<SPListItem>();
			var idField = contentType.GetKeyField();

			if (idField == null)
			{
				throw DataError.OperationRequireIdField();
			}

			var spItem = SpList.AddItem();

			mapper.Map(item, spItem);

			spItem.Update();
		}

		public void Update<T>(T item)
		{
			if (List.IsExternal)
			{
				throw DataError.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<SPListItem>();
			var idField = contentType.GetKeyField();

			if (idField == null)
			{
				throw DataError.OperationRequireIdField();
			}

			var idValue = (int) idField.GetMapper<SPListItem>().MemberAccessor.GetValue(item);

			var spItem = SpList.GetItemById(idValue);

			mapper.Map(item, spItem);

			spItem.Update();
		}

		public void Delete<T>(T item)
		{
			if (List.IsExternal)
			{
				throw DataError.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var idField = contentType.GetKeyField();

			if (idField == null)
			{
				throw DataError.OperationRequireIdField();
			}

			var idValue = (int)idField.GetMapper<SPListItem>().MemberAccessor.GetValue(item);

			var spItem = SpList.GetItemById(idValue);

			spItem.Delete();
		}

		private IList<SPListItem> FetchInternal<T>(QueryModel caml)
		{
			var camlString = ConvertToCamlString<T>(caml);

			return SpList.GetItems(CamlUtility.CamlStringToSPQuery(camlString))
				.Cast<SPListItem>()
				.ToList();
		}

		private T Materialize<T>(SPListItem spItem, IReadOnlyCollection<MemberRefModel> fields = null)
		{
			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<SPListItem>();

			return (T) mapper.CreateAndMap(spItem, fields);
		}
	}
}