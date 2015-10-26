using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Server.Utils;

namespace Untech.SharePoint.Server.Data
{
	internal class SpListItemsProvider : ISpListItemsProvider
	{
		public SpListItemsProvider(SPWeb web, SpCommonService commonService, MetaList list)
		{
			Web = web;
			List = list;
			CommonService = commonService;

			SpList = web.Lists[list.Title];
		}

		public SPWeb Web { get; private set; }

		public SpCommonService CommonService { get; private set; }

		public MetaList List { get; private set; }

		public SPList SpList { get; private set; }

		public IEnumerable<T> Fetch<T>(string caml)
		{
			var viewFields = CamlUtility.GetViewFields(caml);

			return FetchInternal(caml)
				.Select(n => Materialize<T>(n, viewFields));
		}

		public bool Any(string caml)
		{
			return FetchInternal(caml).Any();
		}

		public int Count(string caml)
		{
			return FetchInternal(caml).Count;
		}

		public T SingleOrDefault<T>(string caml)
		{
			var viewFields = CamlUtility.GetViewFields(caml);
			var foundItems = FetchInternal(caml, 2);

			if (foundItems.Count > 1)
			{
				throw Error.MoreThanOneMatch();
			}
			return foundItems.Count == 1 ? Materialize<T>(foundItems[0], viewFields) : default(T);
		}

		public T FirstOrDefault<T>(string caml)
		{
			var viewFields = CamlUtility.GetViewFields(caml);
			var foundItems = FetchInternal(caml, 1);

			return foundItems.Count == 1 ? Materialize<T>(foundItems[0], viewFields) : default(T);
		}

		public T ElementAtOrDefault<T>(string caml, int index)
		{
			var viewFields = CamlUtility.GetViewFields(caml);
			var foundItem = FetchInternal(caml, (uint) (index + 1)).ElementAtOrDefault(index);

			return foundItem != null ? Materialize<T>(foundItem, viewFields) : default(T);
		}

		public T Get<T>(int id)
		{
			if (List.IsExternal)
			{
				throw DataError.OperationNotAllowedForExternalList();
			}

			// NOTE: check contenttype
			return Materialize<T>(SpList.GetItemById(id));
		}

		public void Add<T>(T item)
		{
			if (List.IsExternal)
			{
				throw DataError.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<SPListItem>();
			var idField = contentType.Fields.SingleOrDefault<MetaField>(n => n.InternalName == "ID");

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
			var idField = contentType.Fields.SingleOrDefault<MetaField>(n => n.InternalName == "ID");

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
			var idField = contentType.Fields.SingleOrDefault<MetaField>(n => n.InternalName == "ID");

			if (idField == null)
			{
				throw DataError.OperationRequireIdField();
			}

			var idValue = (int)idField.GetMapper<SPListItem>().MemberAccessor.GetValue(item);

			var spItem = SpList.GetItemById(idValue);

			spItem.Delete();
		}

		private IList<SPListItem> FetchInternal(string caml)
		{
			return SpList.GetItems(CamlUtility.CamlStringToSPQuery(caml))
				.Cast<SPListItem>()
				.ToList();
		}

		private IList<SPListItem> FetchInternal(string caml, uint overrideRowLimit)
		{
			var query = CamlUtility.CamlStringToSPQuery(caml);
			query.RowLimit = overrideRowLimit;

			return SpList.GetItems(query)
				.Cast<SPListItem>()
				.ToList();
		}

		
		private T Materialize<T>(SPListItem spItem, IReadOnlyCollection<string> fields = null)
		{
			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<SPListItem>();

			var item = (T)mapper.TypeCreator();

			mapper.Map(spItem, item, fields);

			return item;
		}
	}
}