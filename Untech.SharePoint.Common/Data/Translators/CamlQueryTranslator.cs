using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Diagnostics;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal static class CamlHelper
	{
		public static XElement CreateView([CanBeNull]XElement xRowLimit, [CanBeNull]XElement xQuery, [CanBeNull]XElement xViewFields)
		{
			return new XElement(Tags.View, xRowLimit, xQuery, xViewFields);
		}

		public static XElement CreateQuery([CanBeNull]XElement xWhere, [CanBeNull]XElement xOrderBy)
		{
			return new XElement(Tags.Query, xWhere, xOrderBy);
		}

		public static XElement CreateWhere(LogicalJoinOperator logicalJoinOperator, [NotNull]XElement xFirst, [NotNull]XElement xSecond)
		{
			return new XElement(logicalJoinOperator.ToString(), xFirst, xSecond);
		}

		public static XElement CreateWhere(ComparisonOperator comparisonOperator, [NotNull]XElement xFieldRef, [CanBeNull]XElement xValue)
		{
			return new XElement(comparisonOperator.ToString(), xFieldRef, xValue);
		}
	}

	internal class CamlQueryTranslator : IProcessor<QueryModel, string>
	{
		public CamlQueryTranslator([NotNull]MetaContentType contentType)
		{
			Guard.CheckNotNull("contentType", contentType);

			ContentType = contentType;
		}

		[NotNull]
		private MetaContentType ContentType { get; set; }

		[NotNull]
		public string Process([NotNull]QueryModel query)
		{
			Guard.CheckNotNull("query", query);

			Logger.Trace(LogCategories.QueryTranslator, "Original QueryModel:\n{0}", query);

			var result = GetQuery(query).ToString();

			Logger.Trace(LogCategories.QueryTranslator, "CAML-string that was generated from QueryModel:\n{0}", result);

			return result;
		}

		[NotNull]
		private XElement GetQuery([NotNull] QueryModel query)
		{
			return CamlHelper.CreateView(
				GetRowLimit(query.RowLimit),
				CamlHelper.CreateQuery(
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
				? new XElement(Tags.ViewFields, models.Select(GetViewField).Distinct(new XNodeEqualityComparer()))
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
			if (comparison.ComparisonOperator == ComparisonOperator.ContainsOrIncludes ||
				comparison.ComparisonOperator == ComparisonOperator.NotContainsOrIncludes)
			{
				return GetContainsOrIncludes(comparison);
			}

			if (comparison.ComparisonOperator == ComparisonOperator.IsNull ||
				comparison.ComparisonOperator == ComparisonOperator.IsNotNull)
			{
				return new XElement(comparison.ComparisonOperator.ToString(),
					new XElement(Tags.FieldRef, GetFieldRefName(comparison.Field)));
			}

			return GetComparison(comparison.ComparisonOperator, comparison.Field, comparison.Value, comparison.IsValueConverted);
		}

		[NotNull]
		private XElement GetContainsOrIncludes([NotNull] ComparisonModel comparison)
		{
			if (comparison.Field.Type != FieldRefType.KnownMember)
			{
				throw new NotSupportedException("Unsupported FieldRefType value");
			}

			var memberRef = (MemberRefModel)comparison.Field;
			var metaField = GetMetaField(memberRef.Member);

			if (comparison.ComparisonOperator == ComparisonOperator.NotContainsOrIncludes)
			{
				if (metaField.TypeAsString.StartsWith("User") || metaField.TypeAsString.StartsWith("Lookup")) 
				{
					return GetComparison(ComparisonOperator.NotIncludes, metaField, comparison.Value, comparison.IsValueConverted);
				}

				throw new NotSupportedException("Cannot negate Contains operation for non-lookup fields");
			}

			if (metaField.TypeAsString.StartsWith("User") || metaField.TypeAsString.StartsWith("Lookup"))
			{
				return GetComparison(ComparisonOperator.Includes, metaField, comparison.Value, comparison.IsValueConverted);
			}

			return GetComparison(ComparisonOperator.Contains, metaField, comparison.Value, comparison.IsValueConverted);
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
					var memberRef = (MemberRefModel) fieldRef;
					return new XAttribute(Tags.Name, GetMetaField(memberRef.Member).InternalName);
			}

			throw new NotSupportedException("Unsupported FieldRefType value");
		}

		[NotNull]
		private XElement GetValue([NotNull]FieldRefModel fieldRef, [CanBeNull]object value, bool alreadyConverted = false)
		{
			if (fieldRef.Type == FieldRefType.KnownMember)
			{
				var memberRef = (MemberRefModel) fieldRef;
				return GetValue(GetMetaField(memberRef.Member), value, alreadyConverted);
			}

			if (!alreadyConverted)
			{
				throw new NotSupportedException("Only already converted values allowed with non FieldRefType.KnownMember field refs");
			}

			return new XElement(Tags.Value, value);
		}

		[NotNull]
		private XElement GetValue([NotNull] MetaField metaField, [CanBeNull] object value, bool alreadyConverted = false)
		{
			var camlValue = alreadyConverted
				? value
				: GetConverter(metaField).ToCamlValue(value);
			var typeAttr = metaField.IsCalculated
				? new XAttribute(Tags.Type, metaField.OutputType)
				: new XAttribute(Tags.Type, metaField.TypeAsString);

			return new XElement(Tags.Value, typeAttr, camlValue);
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

		private IFieldConverter GetConverter([NotNull]MetaField field)
		{
			var converter = field.Converter;
			if (converter == null)
			{
				throw new InvalidOperationException(string.Format("Converter wasn't initialized for '{0}' field", field.MemberName));
			}
			return converter;
		}

		[NotNull]
		private XElement GetComparison(ComparisonOperator comparisonOperator, [NotNull] FieldRefModel fieldRef, object value,
			bool alreadyConverted = false)
		{
			if (fieldRef.Type != FieldRefType.KnownMember)
			{
				return CamlHelper.CreateWhere(comparisonOperator,
					new XElement(Tags.FieldRef, GetFieldRefName(fieldRef)),
					GetValue(fieldRef, value, alreadyConverted));
			}

			var memberRef = (MemberRefModel) fieldRef;
			return GetComparison(comparisonOperator, GetMetaField(memberRef.Member), value, alreadyConverted);
		}

		[NotNull]
		private XElement GetComparison(ComparisonOperator comparisonOperator, [NotNull] MetaField metaField, object value,
			bool alreadyConverted = false)
		{
			var xFieldRef = new XElement(Tags.FieldRef, new XAttribute(Tags.Name, metaField.InternalName));
			if (metaField.TypeAsString.StartsWith("User") || metaField.TypeAsString.StartsWith("Lookup"))
			{
				xFieldRef.Add(new XAttribute("LookupId", "TRUE"));
			}
			var xValue = GetValue(metaField, value, alreadyConverted);

			return CamlHelper.CreateWhere(comparisonOperator, xFieldRef, xValue);
		}
	}
}