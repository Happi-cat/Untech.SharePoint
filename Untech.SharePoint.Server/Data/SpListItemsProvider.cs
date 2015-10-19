using System.Collections.Generic;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Server.Data
{
	internal class SpListItemsProvider : ISpListItemsProvider
	{
		public SpListItemsProvider(SPWeb web, SpCommonService commonService, MetaList list)
		{
			Web = web;
			List = list;
			CommonService = commonService;
		}

		public SPWeb Web { get; private set; }

		public SpCommonService CommonService { get; private set; }

		public MetaList List { get; private set; }

		public IEnumerable<T> Fetch<T>(string caml)
		{
			throw new System.NotImplementedException();
		}

		public bool Any(string caml)
		{
			throw new System.NotImplementedException();
		}

		public int Count(string caml)
		{
			throw new System.NotImplementedException();
		}

		public T SingleOrDefault<T>(string caml)
		{
			throw new System.NotImplementedException();
		}

		public T FirstOrDefault<T>(string caml)
		{
			throw new System.NotImplementedException();
		}

		public T ElementAtOrDefault<T>(string caml, int index)
		{
			throw new System.NotImplementedException();
		}

		public void Add<T>(T item)
		{
			throw new System.NotImplementedException();
		}

		public void Update<T>(T item)
		{
			throw new System.NotImplementedException();
		}
	}
}