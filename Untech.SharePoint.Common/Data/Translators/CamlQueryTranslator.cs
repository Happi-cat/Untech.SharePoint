using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal class CamlQueryTranslator
	{
		public CamlQueryTranslator(MetaContentType contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
		}

		public MetaContentType ContentType { get; private set; }

		public string Translate(QueryModel query)
		{
			return GetQuery(query).ToString();
		}

		protected XElement GetQuery(QueryModel query)
		{
			return new XElement(Tags.Query,
				GetRowLimit(query.RowLimit),
				GetViewFields(query.SelectableFields),
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

			if (!string.IsNullOrEmpty(ContentType.Id))
			{
				xWhere = AppendContentTypeFilter(xWhere);
			}

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
			if (comparison != null)
			{
				return GetComparison(comparison);
			}

			throw new NotSupportedException(string.Format("'{0}' is not supported", @where));
		}

		protected XElement GetOrderBys(IEnumerable<OrderByModel> orderBys, bool isOrderReversed)
		{
			if (orderBys == null)
			{
				return null;
			}

			var models = orderBys.ToList();
			if (models.Any())
			{
				return isOrderReversed
					? new XElement(Tags.OrderBy, models.Select(n => n.Reverse()).Select(GetOrderBy))
					: new XElement(Tags.OrderBy, models.Select(GetOrderBy));
			}
			if (isOrderReversed)
			{
				var key = ContentType.List.IsExternal ? Fields.BdcIdentity : Fields.Id;
				return new XElement(Tags.OrderBy,
					new XElement(Tags.FieldRef,
						new XAttribute(Tags.Name, key),
						new XAttribute(Tags.Ascending, false)));
			}

			return null;
		}

		protected XElement GetOrderBy(OrderByModel orderBy)
		{
			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Ascending, orderBy.Ascending.ToString().ToUpper()),
				GetFieldRefName(orderBy.FieldRef));
		}
		protected XElement GetViewFields(IEnumerable<FieldRefModel> fieldRefs)
		{
			if (fieldRefs == null)
			{
				return null;
			}

			var models = fieldRefs.ToList();
			return models.Any()
				? new XElement(Tags.ViewFields, models.Select(GetViewField))
				: null;
		}

		protected XElement GetViewField(FieldRefModel fieldRef)
		{
			return new XElement(Tags.FieldRef, GetFieldRefName(fieldRef));
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
				GetValue(comparison.Field, comparison.Value, comparison.IsValueConverted));
		}

		protected XAttribute GetFieldRefName(FieldRefModel fieldRef)
		{
			if (fieldRef.Type == FieldRefType.Key)
			{
				var key = ContentType.List.IsExternal ? Fields.BdcIdentity : Fields.Id;
				
				return new XAttribute(Tags.Name, key);
			}
			return new XAttribute(Tags.Name, GetMetaField(fieldRef.Member).InternalName);
		}

		protected XElement GetValue(FieldRefModel fieldRef, object value, bool alreadyConverted = false)
		{
			if (fieldRef.Type != FieldRefType.KnownMember)
			{
				throw new NotSupportedException();
			}

			return new XElement(Tags.Value,
				new XAttribute(Tags.Type, GetMetaField(fieldRef.Member).TypeAsString),
				alreadyConverted ? value : GetConverter(fieldRef.Member).ToCamlValue(value));
		}

		private XElement AppendContentTypeFilter(XElement xWhere)
		{
			if (ContentType.List.IsExternal)
			{
				return xWhere;
			}

			var xContentType = new XElement(Tags.BeginsWith,
					new XElement(Tags.FieldRef, new XAttribute(Tags.Name, Fields.ContentTypeId)),
					new XElement(Tags.Value, ContentType.Id));

			if (xWhere != null)
			{
				return new XElement(Tags.And,
					xContentType,
					xWhere);
			}

			return xContentType;
		}

		private MetaField GetMetaField(MemberInfo member)
		{
			return ContentType.Fields[member.Name];
		}

		private IFieldConverter GetConverter(MemberInfo member)
		{
			return GetMetaField(member).Converter;
		}
	}
}