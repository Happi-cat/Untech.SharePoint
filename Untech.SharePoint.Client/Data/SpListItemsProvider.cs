using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Utils;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Client.Data
{
	internal class SpListItemsProvider : BaseSpListItemsProvider, ISpListItemsProvider
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

		public IEnumerable<T> Fetch<T>(QueryModel caml)
		{
			var viewFields = caml.SelectableFields.EmptyIfNull().ToList();

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
			var viewFields = caml.SelectableFields.EmptyIfNull().ToList();

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
			var viewFields = caml.SelectableFields.EmptyIfNull().ToList();

			caml.RowLimit = 1;

			var foundItems = FetchInternal<T>(caml);

			return foundItems.Count == 1 ? Materialize<T>(foundItems[0], viewFields) : default(T);
		}

		public T ElementAtOrDefault<T>(QueryModel caml, int index)
		{
			var viewFields = caml.SelectableFields.EmptyIfNull().ToList();

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

			var spItem = SpList.GetItemById(id);
			ClientContext.Load(spItem);
			ClientContext.ExecuteQuery();

			return Materialize<T>(spItem);
		}

		public void Add<T>(T item)
		{
			if (List.IsExternal)
			{
				throw DataError.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<ListItem>();
			var idField = contentType.GetKeyField();

			if (idField == null)
			{
				throw DataError.OperationRequireIdField();
			}

			var info = new ListItemCreationInformation();
			var spItem = SpList.AddItem(info);

			mapper.Map(item, spItem);

			spItem.Update();
			ClientContext.ExecuteQuery();

		}

		public void Update<T>(T item)
		{
			if (List.IsExternal)
			{
				throw DataError.OperationNotAllowedForExternalList();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<ListItem>();
			var idField = contentType.GetKeyField();

			if (idField == null)
			{
				throw DataError.OperationRequireIdField();
			}

			var idValue = (int)idField.GetMapper<ListItem>().MemberAccessor.GetValue(item);

			var spItem = SpList.GetItemById(idValue);
			ClientContext.Load(spItem);
			ClientContext.ExecuteQuery();

			mapper.Map(item, spItem);

			spItem.Update();
			ClientContext.ExecuteQuery();
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

			var idValue = (int)idField.GetMapper<ListItem>().MemberAccessor.GetValue(item);
			
			var spItem = SpList.GetItemById(idValue);
			ClientContext.Load(spItem);
			ClientContext.ExecuteQuery();
			
			spItem.DeleteObject();
			ClientContext.ExecuteQuery();
		}

		private IList<ListItem> FetchInternal<T>(QueryModel caml)
		{
			var camlString = ConvertToCamlString<T>(caml);
			var listCollection = SpList.GetItems(CamlUtility.CamlStringToSPQuery(camlString));

			ClientContext.Load(listCollection);
			ClientContext.ExecuteQuery();

			return listCollection.Cast<ListItem>().ToList();
		}

		private T Materialize<T>(ListItem spItem, IReadOnlyCollection<MemberRefModel> fields = null)
		{
			var contentType = List.ContentTypes[typeof (T)];
			var mapper = contentType.GetMapper<ListItem>();

			return (T)mapper.CreateAndMap(spItem, fields);
		}
	}
}