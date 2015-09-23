using System.Collections.Generic;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class QueryModel
	{
		public uint RowLimit { get; set; }

		public WhereModel WhereModel { get; set; }

		public IEnumerable<FieldRefModel> SelectableFields { get; set; }

		public IEnumerable<OrderByModel> OrderBys { get; set; }

		public IEnumerable<GroupByModel> GroupBys { get; set; }
	}
}