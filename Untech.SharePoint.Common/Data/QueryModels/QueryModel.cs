using System.Collections.Generic;
using System.Linq;
using System.Text;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class QueryModel
	{
		public int? RowLimit { get; set; }

		public WhereModel Where { get; set; }

		public IEnumerable<FieldRefModel> SelectableFields { get; set; }

		public IEnumerable<OrderByModel> OrderBys { get; set; }

		public bool IsOrderReversed { get; set; }

		public void MergeWheres(WhereModel where)
		{
			if (where == null)
			{
				return;
			}
			Where = Where == null ? @where : new LogicalJoinModel(LogicalJoinOperator.And, Where, @where);
		}

		public void MergeOrderBys(OrderByModel orderBy)
		{
			var newOrderBys = new List<OrderByModel>();
			if (OrderBys != null)
			{
				newOrderBys.AddRange(OrderBys);
			}
			if (orderBy != null)
			{
				newOrderBys.Add(IsOrderReversed ? orderBy.Reverse() : orderBy);
			}
			OrderBys = newOrderBys;
		}

		public void MergeOrderBys(IEnumerable<OrderByModel> orderBys)
		{
			var newOrderBys = new List<OrderByModel>();
			if (OrderBys != null)
			{
				newOrderBys.AddRange(OrderBys);
			}
			if (orderBys != null)
			{
				newOrderBys.AddRange(IsOrderReversed ? orderBys.Select(n => n.Reverse()) : orderBys);
			}
			OrderBys = newOrderBys;
		}

		public void MergeSelectableFields(IEnumerable<FieldRefModel> selectableFields)
		{
			var newSelectableFields = new List<FieldRefModel>();
			if (SelectableFields != null)
			{
				newSelectableFields.AddRange(SelectableFields);
			}
			if (selectableFields != null)
			{
				newSelectableFields.AddRange(selectableFields);
			}
			SelectableFields = newSelectableFields.Distinct(FieldRefModelComparer.Comparer);
		}

		public void ReverseOrder()
		{
			IsOrderReversed = !IsOrderReversed;
		}

		public void ResetOrder()
		{
			IsOrderReversed = false;
			OrderBys = null;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("<Query>");
			if (RowLimit != null)
			{
				sb.AppendFormat("<RowLimit>{0}</RowLimit>", RowLimit);
			}
			if (Where != null)
			{
				sb.AppendFormat("<Where>{0}</Where>", Where);
			}
			if (OrderBys != null && OrderBys.Any())
			{
				sb.AppendFormat("<OrderBy>{0}</OrderBy>", IsOrderReversed
					? OrderBys.Select(n => n.Reverse()).JoinToString("")
					: OrderBys.JoinToString(""));
			}
			else if (IsOrderReversed)
			{
				sb.Append("<OrderBy><FieldRef Name='ID' Ascending='FALSE' /></OrderBy>");
			}
			if (SelectableFields != null && SelectableFields.Any())
			{
				sb.AppendFormat("<ViewFields>{0}</ViewFields>", SelectableFields.JoinToString(""));
			}
			sb.Append("</Query>");

			return sb.ToString();
		}
	}
}