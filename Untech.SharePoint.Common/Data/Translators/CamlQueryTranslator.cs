using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Diagnostics;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal class CamlQueryTranslator : IProcessor<QueryModel, string>
	{
		public CamlQueryTranslator([NotNull]MetaContentType contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
		}

		[NotNull]
		public MetaContentType ContentType { get; private set; }

		[NotNull]
		public string Process([NotNull]QueryModel query)
		{
			Guard.CheckNotNull("query", query);

			Logger.Log(LogLevel.Trace, LogCategories.QueryTranslator, 
				"Original QueryModel:\n{0}", query);

			var result = GetQuery(query).ToString();

			Logger.Log(LogLevel.Trace, LogCategories.QueryTranslator, 
				"CAML-string that was generated from QueryModel:\n{0}", result);

			return result;
		}

		[NotNull]
		private XElement GetQuery([NotNull] QueryModel query)
		{
			return new XElement(Tags.View,
				GetRowLimit(query.RowLimit),
				new XElement(Tags.Query,
					GetWheres(query.Where),
					GetOrderBys(query.OrderBys)),
				GetViewFields(query.SelectableFields));
		}

		[CanBeNull]
		private XElement GetRowLimit(int? rowLimit)
		{
			return rowLimit != null ? new XElement(Tags.RowLimit, rowLimit) : null;
		}

		[CanBeNull]
		private XElement GetWheres([CanBeNull]WhereModel where)
		{
			var xWhere = GetWhere(where);

			return xWhere != null ? new XElement(Tags.Where, xWhere) : null;
		}

		[CanBeNull]
		private XElement GetWhere([CanBeNull]WhereModel @where)
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
		private XElement GetOrderBys([CanBeNull][ItemNotNull] IEnumerable<OrderByModel> orderBys)
		{
			if (orderBys == null)
			{
				return null;
			}

			var models = orderBys.ToList();
			return models.Any() ? new XElement(Tags.OrderBy, models.Select(GetOrderBy)) : null;
		}

		[NotNull]
		private XElement GetOrderBy([NotNull]OrderByModel orderBy)
		{
			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Ascending, orderBy.Ascending.ToString().ToUpper()),
				GetFieldRefName(orderBy.FieldRef));
		}

		[CanBeNull]
		private XElement GetViewFields([CanBeNull][ItemNotNull] IEnumerable<FieldRefModel> fieldRefs)
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
		private XElement GetViewField([NotNull]FieldRefModel fieldRef)
		{
			return new XElement(Tags.FieldRef, GetFieldRefName(fieldRef));
		}

		[NotNull]
		private XElement GetLogicalJoin([NotNull]LogicalJoinModel logicalJoin)
		{
			return new XElement(logicalJoin.LogicalOperator.ToString(),
				GetWhere(logicalJoin.First),
				GetWhere(logicalJoin.Second));
		}

		[NotNull]
		private XElement GetComparison([NotNull]ComparisonModel comparison)
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
		private XAttribute GetFieldRefName([NotNull]FieldRefModel fieldRef)
		{
			switch (fieldRef.Type)
			{
				case FieldRefType.Key:
					var key = ContentType.List.IsExternal ? Fields.BdcIdentity : Fields.Id;
					return new XAttribute(Tags.Name, key);
				case FieldRefType.ContentTypeId:
					return new XAttribute(Tags.Name, Fields.ContentTypeId);
				case FieldRefType.KnownMember:
					var memberRef = (MemberRefModel)fieldRef;
					return new XAttribute(Tags.Name, GetMetaField(memberRef.Member).InternalName);
			}

			throw new NotSupportedException("Unsupported FieldRefType value");
		}

		[NotNull]
		private XElement GetValue([NotNull]FieldRefModel fieldRef, [CanBeNull]object value, bool alreadyConverted = false)
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