﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Utils;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Client.Data
{
	internal class SpListItemsProvider : ISpListItemsProvider
	{
		public SpListItemsProvider(ClientContext clientContext, SpCommonService commonService, MetaList list)
		{
			ClientContext = clientContext;
			List = list;
			CommonService = commonService;

			SpList = clientContext.GetList(list.Title);
		}

		public ClientContext ClientContext { get; private set; }

		public SpCommonService CommonService { get; private set; }

		public MetaList List { get; private set; }

		public List SpList { get; private set; }

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
				throw new InvalidOperationException("Multiple items match");
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
				throw new InvalidOperationException();
			}

			// NOTE: check contenttype

			return Materialize<T>(SpList.GetItemById(id));
		}

		public void Add<T>(T item)
		{
			if (List.IsExternal)
			{
				throw new InvalidOperationException();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<ListItem>();
			var idField = contentType.Fields.SingleOrDefault<MetaField>(n => n.InternalName == "ID");

			if (idField == null)
			{
				throw new InvalidOperationException();
			}

			throw new NotImplementedException();

		}

		public void Update<T>(T item)
		{
			if (List.IsExternal)
			{
				throw new InvalidOperationException();
			}

			var contentType = List.ContentTypes[typeof(T)];
			var mapper = contentType.GetMapper<ListItem>();
			var idField = contentType.Fields.SingleOrDefault<MetaField>(n => n.InternalName == "ID");

			if (idField == null)
			{
				throw new InvalidOperationException();
			}

			throw new NotImplementedException();
		}

		public void Delete<T>(T item)
		{
			throw new NotImplementedException();
		}

		private IList<ListItem> FetchInternal(string caml)
		{
			var listCollection = SpList.GetItems(CamlUtility.CamlStringToSPQuery(caml));

			ClientContext.Load(listCollection);
			ClientContext.ExecuteQuery();

			return listCollection.Cast<ListItem>().ToList();
		}

		private IList<ListItem> FetchInternal(string caml, uint overrideRowLimit)
		{
			var listCollection = SpList.GetItems(CamlUtility.CamlStringToSPQuery(caml, overrideRowLimit));

			ClientContext.Load(listCollection);
			ClientContext.ExecuteQuery();

			return listCollection.Cast<ListItem>().ToList();
		}

		private T Materialize<T>(ListItem spItem, IReadOnlyCollection<string> fields = null)
		{
			var contentType = List.ContentTypes[typeof (T)];
			var mapper = contentType.GetMapper<ListItem>();

			var item = (T) mapper.TypeCreator();

			mapper.Map(spItem, item, fields);

			return item;
		}


	}
}