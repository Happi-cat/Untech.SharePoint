using System.Collections.Generic;
using System.Text;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class QueryModel
	{
		public uint RowLimit { get; set; }

		public WhereModel Where { get; set; }

		public IEnumerable<FieldRefModel> SelectableFields { get; set; }

		public IEnumerable<OrderByModel> OrderBys { get; set; }

		public IEnumerable<FieldRefModel> GroupBys { get; set; }

		public void MergeWheres(WhereModel where)
		{
			if (where == null)
				return;
			Where = Where == null ? @where : new LogicalJoinModel(LogicalJoinOperator.And, Where, @where);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("<Query>");

			sb.AppendFormat("<RowLimit>{0}</RowLimit>", RowLimit);

			if (Where != null)
			{
				sb.AppendFormat("<Where>{0}</Where>", Where);
			}

			sb.Append("</Query>");

			return sb.ToString();
		}
	}
}