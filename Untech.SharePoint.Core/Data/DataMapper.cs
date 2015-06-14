using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Core.Data.Fields.Converters;

namespace Untech.SharePoint.Core.Data
{
    public class DataMapper<TModel>
    {
		public DataMapper()
		{
			
		}

		public void Map(SPListItem sourceItem, TModel destItem)
		{
			throw new NotImplementedException();
		}

		public void Map(TModel sourceItem, SPListItem destItem)
		{
			throw new NotImplementedException();
		}
    }
}
