using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.FieldConverters;

namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaModel
	{
		public abstract MetaList GetList(string listTitle);

		public abstract IEnumerable<MetaList> GetLists();

	}
}