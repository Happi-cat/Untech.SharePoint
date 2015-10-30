using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Represents CAML Query tag.
	/// </summary>
	public sealed class QueryModel
	{
		/// <summary>
		/// Gets or sets query row limit value.
		/// </summary>
		[CanBeNull]
		public int? RowLimit { get; set; }

		/// <summary>
		/// Gets inner operator for CAML Where tag.
		/// </summary>
		[CanBeNull]
		public WhereModel Where { get; private set; }

		/// <summary>
		/// Gets collection of specific selectable fields.
		/// </summary>
		[CanBeNull]
		public IEnumerable<FieldRefModel> SelectableFields { get; private set; }

		/// <summary>
		/// Gets collection of orderings for CAML OrderBy tag. Ordering can be direct or reversed. <seealso cref="IsOrderReversed"/>.
		/// </summary>
		[CanBeNull]
		public IEnumerable<OrderByModel> OrderBys { get; private set; }

		/// <summary>
		/// Determines whether <see cref="OrderBys"/> is in direct ordering or is in reversed.
		/// </summary>
		public bool IsOrderReversed { get; private set; }

		/// <summary>
		/// Merge current CAML where operation with new one.
		/// </summary>
		/// <param name="where">New Where operation to merge.</param>
		public void MergeWheres([CanBeNull]WhereModel where)
		{
			Where = WhereModel.And(Where, @where);
		}

		/// <summary>
		/// Merge current CAML Orderby operations with new one.
		/// </summary>
		/// <param name="orderBy">New OrderBy operation to merge.</param>
		public void MergeOrderBys([CanBeNull]OrderByModel orderBy)
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

		/// <summary>
		/// Merge current CAML Orderby operations with new ones.
		/// </summary>
		/// <param name="orderBys">New OrderBy operations to merge.</param>
		public void MergeOrderBys([CanBeNull]IEnumerable<OrderByModel> orderBys)
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

		/// <summary>
		/// Merge current CAML selectable fields with new ones.
		/// </summary>
		/// <param name="selectableFields">New selectable fields to merge.</param>
		public void MergeSelectableFields([CanBeNull]IEnumerable<FieldRefModel> selectableFields)
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

		/// <summary>
		/// Reverses current ordering operations.
		/// </summary>
		public void ReverseOrder()
		{
			IsOrderReversed = !IsOrderReversed;
		}

		/// <summary>
		/// Resets ordering to default.
		/// </summary>
		public void ResetOrder()
		{
			IsOrderReversed = false;
			OrderBys = null;
		}

		/// <summary>
		/// Returns a <see cref="string"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
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
			if (SelectableFields != null && SelectableFields.Any())
			{
				sb.AppendFormat("<ViewFields>{0}</ViewFields>", SelectableFields.JoinToString(""));
			}
			sb.Append("</Query>");

			return sb.ToString();
		}
	}
}