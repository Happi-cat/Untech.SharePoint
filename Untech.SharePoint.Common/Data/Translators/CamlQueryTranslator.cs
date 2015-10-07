using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal class CamlQueryTranslator
	{
		public CamlQueryTranslator(MetaList list)
		{
			List = list;
		}

		public MetaList List { get; set; }
		public string Translate(QueryModel query)
		{
			return GetQuery(query).ToString();
		}

		protected XElement GetQuery(QueryModel query)
		{
			return new XElement(Tags.Query,
				GetRowLimit(query.RowLimit),
				GetWheres(query.Where),
				GetOrderBys(query.OrderBys, query.IsOrderReversed));
		}

		protected XElement GetRowLimit(int? rowLimit)
		{
			return rowLimit != null ? new XElement(Tags.RowLimit, rowLimit) : null;
		}

		protected XElement GetWheres(WhereModel where)
		{
			var xWhere = GetWhere(where);

			return xWhere != null ? new XElement(Tags.Where, xWhere) : null;
		}

		protected XElement GetWhere(WhereModel @where)
		{
			if (@where == null)
			{
				return null;
			}

			var join = @where as LogicalJoinModel;
			if (@join != null)
			{
				return GetLogicalJoin(@join);
			}

			var comparison = @where as ComparisonModel;
			return comparison != null ? GetComparison(comparison) : null;
		}

		protected XElement GetOrderBys(IEnumerable<OrderByModel> orderBys, bool isOrderReversed)
		{
			var models = orderBys.ToList();
			if (models.Any())
			{
				return isOrderReversed 
					? new XElement(Tags.OrderBy, models.Select(n => n.Reverse()).Select(GetOrderBy)) 
					: new XElement(Tags.OrderBy, models.Select(GetOrderBy));
			}
			
			return new XElement(Tags.OrderBy,
				new XElement(Tags.FieldRef,
					new XAttribute(Tags.Ascending, (!isOrderReversed).ToString().ToUpper()),
					new XAttribute(Tags.Name, "ID")));
		}

		protected XElement GetOrderBy(OrderByModel orderBy)
		{
			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Ascending, orderBy.Ascending.ToString().ToUpper()),
				GetFieldRefName(orderBy.Field));
		}

		protected XElement GetLogicalJoin(LogicalJoinModel logicalJoin)
		{
			return new XElement(logicalJoin.LogicalOperator.ToString(), 
				GetWhere(logicalJoin.First),
				GetWhere(logicalJoin.Second));
		}

		protected XElement GetComparison(ComparisonModel comparison)
		{
			if (comparison.ComparisonOperator == ComparisonOperator.IsNull ||
			    comparison.ComparisonOperator == ComparisonOperator.IsNotNull)
			{
				return new XElement(comparison.ComparisonOperator.ToString(), 
					new XElement(Tags.FieldRef, GetFieldRefName(comparison.Field)));
			}

			return new XElement(comparison.ComparisonOperator.ToString(),
				new XElement(Tags.FieldRef, GetFieldRefName(comparison.Field)),
				GetValue(comparison.Field, comparison.Value));
		}

		protected XAttribute GetFieldRefName(FieldRefModel fieldRef)
		{
			return new XAttribute(Tags.Name, GetMetaField(fieldRef.Member).InternalName);
		}

		protected XElement GetValue(FieldRefModel fieldRef, object value)
		{
			return new XElement(Tags.Value,
				new XAttribute(Tags.Type, GetMetaField(fieldRef.Member).TypeAsString),
				GetConverter(fieldRef.Member).ToCamlValue(value));
		}

		private MetaField GetMetaField(MemberInfo member)
		{
			throw new NotImplementedException();
		}

		private IFieldConverter GetConverter(MemberInfo member)
		{
			throw new NotImplementedException();
		}
	}
}