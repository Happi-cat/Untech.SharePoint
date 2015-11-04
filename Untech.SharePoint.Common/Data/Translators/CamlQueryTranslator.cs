using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal class CamlQueryTranslator
	{
		public CamlQueryTranslator([NotNull]MetaContentType contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
		}

		[NotNull]
		public MetaContentType ContentType { get; private set; }

		[NotNull]
		public string Translate([NotNull]QueryModel query)
		{
			Guard.CheckNotNull("query", query);

			return GetQuery(query).ToString();
		}

		[NotNull]
		protected XElement GetQuery([NotNull] QueryModel query)
		{
			return new XElement(Tags.View,
				GetRowLimit(query.RowLimit),
				new XElement(Tags.Query,
					GetWheres(query.Where),
					GetOrderBys(query.OrderBys, query.IsOrderReversed)),
				GetViewFields(query.SelectableFields));
		}

		[CanBeNull]
		protected XElement GetRowLimit(int? rowLimit)
		{
			return rowLimit != null ? new XElement(Tags.RowLimit, rowLimit) : null;
		}

		[CanBeNull]
		protected XElement GetWheres([CanBeNull]WhereModel where)
		{
			var xWhere = GetWhere(where);

			if (!string.IsNullOrEmpty(ContentType.Id))
			{
				xWhere = AppendContentTypeFilter(xWhere);
			}

			return xWhere != null ? new XElement(Tags.Where, xWhere) : null;
		}

		[CanBeNull]
		protected XElement GetWhere([CanBeNull]WhereModel @where)
		{
			if (@where == null)
			{
				return null;
			}

			switch (@where.Type)
			{
				case WhereType.LogicalJoin:
					return GetLogicalJoin((LogicalJoinModel)@where);
				case WhereType.Comparison:
					return GetComparison((ComparisonModel)@where);
			}

			throw new NotSupportedException(string.Format("'{0}' is not supported", @where));
		}

		[CanBeNull]
		protected XElement GetOrderBys([CanBeNull]IEnumerable<OrderByModel> orderBys, bool isOrderReversed)
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

			return null;
		}

		[NotNull]
		protected XElement GetOrderBy([NotNull]OrderByModel orderBy)
		{
			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Ascending, orderBy.Ascending.ToString().ToUpper()),
				GetFieldRefName(orderBy.FieldRef));
		}

		[CanBeNull]
		protected XElement GetViewFields([CanBeNull]IEnumerable<FieldRefModel> fieldRefs)
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

		[NotNull]
		protected XElement GetViewField([NotNull]FieldRefModel fieldRef)
		{
			return new XElement(Tags.FieldRef, GetFieldRefName(fieldRef));
		}

		[NotNull]
		protected XElement GetLogicalJoin([NotNull]LogicalJoinModel logicalJoin)
		{
			return new XElement(logicalJoin.LogicalOperator.ToString(),
				GetWhere(logicalJoin.First),
				GetWhere(logicalJoin.Second));
		}

		[NotNull]
		protected XElement GetComparison([NotNull]ComparisonModel comparison)
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

		[NotNull]
		protected XAttribute GetFieldRefName([NotNull]FieldRefModel fieldRef)
		{
			if (fieldRef.Type == FieldRefType.Key)
			{
				var key = ContentType.List.IsExternal ? Fields.BdcIdentity : Fields.Id;

				return new XAttribute(Tags.Name, key);
			}
			if (fieldRef.Type == FieldRefType.KnownMember)
			{
				var memberRef = (MemberRefModel)fieldRef;
				return new XAttribute(Tags.Name, GetMetaField(memberRef.Member).InternalName);
			}

			throw new NotSupportedException("Unsupported FieldRefType value");
		}

		[NotNull]
		protected XElement GetValue([NotNull]FieldRefModel fieldRef, [CanBeNull]object value, bool alreadyConverted = false)
		{
			if (fieldRef.Type != FieldRefType.KnownMember)
			{
				throw new NotSupportedException("Only FieldRefType.KnownMember supported by GetValue method");
			}

			var memberRef = (MemberRefModel)fieldRef;

			return new XElement(Tags.Value,
				new XAttribute(Tags.Type, GetMetaField(memberRef.Member).TypeAsString),
				alreadyConverted ? value : GetConverter(memberRef.Member).ToCamlValue(value));
		}

		[CanBeNull]
		private XElement AppendContentTypeFilter([CanBeNull]XElement xWhere)
		{
			if (ContentType.List.IsExternal)
			{
				return xWhere;
			}

			var xContentType = new XElement(Tags.Eq,
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

		[NotNull]
		private MetaField GetMetaField([NotNull]MemberInfo member)
		{
			if (!ContentType.Fields.ContainsKey(member.Name))
			{
				throw new InvalidOperationException(string.Format("'{0}' wasn't mapped and cannot be used in CAML query.", member));
			}

			return ContentType.Fields[member.Name];
		}

		private IFieldConverter GetConverter([NotNull]MemberInfo member)
		{
			var converter = GetMetaField(member).Converter;
			if (converter == null)
			{
				throw new InvalidOperationException(string.Format("Converter wasn't initialized for '{0}' field", member));
			}
			return converter;
		}
	}
}