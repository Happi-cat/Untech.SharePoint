using System.Collections.Generic;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class QueryModel
	{
		public uint RowLimit { get; set; }

		public WhereModel Where { get; set; }

		public IEnumerable<FieldRefModel> SelectableFields { get; set; }

		public IEnumerable<OrderByModel> OrderBys { get; set; }

		public IEnumerable<GroupByModel> GroupBys { get; set; }

		public void MergeWheres(WhereModel where)
		{
			if (where == null)
				return;
			Where = Where == null ? @where : new LogicalJoinModel(LogicalJoinOperator.And, Where, @where);
		}
	}
}