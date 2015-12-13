using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Server.Utils;

namespace Untech.SharePoint.Server.Data
{
	internal class SpListItemsProvider : BaseSpListItemsProvider<SPListItem>
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

		protected override IList<SPListItem> FetchInternal(string caml)
	    {
            return SpList.GetItems(CamlUtility.CamlStringToSPQuery(caml))
                .Cast<SPListItem>()
                .ToList();
	    }

	    protected override SPListItem GetInternal(int id, MetaContentType contentType)
	    {
            var spItem = SpList.GetItemById(id);

            if (spItem.ContentTypeId.ToString() != contentType.Id)
            {
                throw new InvalidOperationException("ContentType mismatch");
            }

            return spItem;
	    }

	    protected override void AddInternal(object item, MetaContentType contentType)
	    {
            var mapper = contentType.GetMapper<SPListItem>();
            var spItem = SpList.AddItem();

            mapper.Map(item, spItem);

            spItem.Update();
	    }

	    protected override void UpdateInternal(int id, object item, MetaContentType contentType)
	    {
            var mapper = contentType.GetMapper<SPListItem>();
            var spItem = SpList.GetItemById(id);

            mapper.Map(item, spItem);

            spItem.Update();
	    }

	    protected override void DeleteInternal(int id, MetaContentType contentType)
	    {
            var spItem = SpList.GetItemById(id);

            spItem.Delete();
	    }
	}
}