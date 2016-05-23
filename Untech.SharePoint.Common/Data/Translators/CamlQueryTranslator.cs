using System;
using System.Linq;
using System.Xml.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Diagnostics;
using Untech.SharePoint.Common.Extensions;
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
		private MetaContentType ContentType { get; set; }

		[NotNull]
		public string Process([NotNull]QueryModel query)
		{
			Guard.CheckNotNull("query", query);

			Logger.Trace(LogCategories.QueryTranslator, "Original QueryModel:\n{0}", query);

			var result = View(query).ToString();

			Logger.Trace(LogCategories.QueryTranslator, "CAML-string that was generated from QueryModel:\n{0}", result);

			return result;
		}

		[NotNull]
		private XElement View([NotNull]QueryModel queryModel)
		{
			XElement xRowLimit = null;
			XElement xViewFields = null;

			if (queryModel.RowLimit != null)
			{
				xRowLimit = new XElement(Tags.RowLimit, queryModel.RowLimit);
			}
			var viewFields = queryModel.SelectableFields.EmptyIfNull().ToList();
			if (viewFields.Any())
			{
				xViewFields = new XElement(Tags.ViewFields, 
					viewFields.Select(FieldRef).Distinct(new XNodeEqualityComparer()));
			}

			return new XElement(Tags.View, 
				xRowLimit, 
				Query(queryModel), 
				xViewFields);
		}

		[NotNull]
		private XElement Query([NotNull]QueryModel queryModel)
		{
			XElement xWhere = null;
			XElement xOrderBy = null;

			if (queryModel.Where != null)
			{
				xWhere = new XElement(Tags.Where, Where(queryModel.Where));
			}
			var orderFields = queryModel.OrderBys.EmptyIfNull().ToList();
			if (orderFields.Any())
			{
				xOrderBy = new XElement(Tags.OrderBy, orderFields.Select(FieldRef));
			}

			return new XElement(Tags.Query, xWhere, xOrderBy);
		}

		#region [Where]

		[NotNull]
		private XElement Where([NotNull] WhereModel @where)
		{
			switch (@where.Type)
			{
				case WhereType.LogicalJoin:
					return Where((LogicalJoinModel) @where);
				case WhereType.Comparison:
					return Where((ComparisonModel) @where);
			}

			throw new NotSupportedException(string.Format("'{0}' is not supported", @where));
		}

		[NotNull]
		private XElement Where([NotNull] LogicalJoinModel logicalJoin)
		{
			return Where(logicalJoin.LogicalOperator,
				Where(logicalJoin.First),
				Where(logicalJoin.Second));
		}

		[NotNull]
		private XElement Where([NotNull] ComparisonModel comparison)
		{
			if (comparison.ComparisonOperator == ComparisonOperator.ContainsOrIncludes ||
			    comparison.ComparisonOperator == ComparisonOperator.NotContainsOrIncludes)
			{
				return WhereContainsOrIncludes(comparison);
			}

			if (comparison.ComparisonOperator == ComparisonOperator.IsNull ||
			    comparison.ComparisonOperator == ComparisonOperator.IsNotNull)
			{
				return Where(comparison.ComparisonOperator,
					FieldRef(comparison.Field),
					null);
			}

			return WhereComparison(comparison.ComparisonOperator, comparison.Field, comparison.Value, comparison.IsValueConverted);
		}

		[NotNull]
		private XElement Where(LogicalJoinOperator logicalJoinOperator, [NotNull] XElement xFirst, [NotNull] XElement xSecond)
		{
			return new XElement(logicalJoinOperator.ToString(), xFirst, xSecond);
		}

		[NotNull]
		private XElement Where(ComparisonOperator comparisonOperator, [NotNull] XElement xFieldRef,
			[CanBeNull] XElement xValue)
		{
			return new XElement(comparisonOperator.ToString(), xFieldRef, xValue);
		}

		#endregion


		#region [FieldRef]

		[NotNull]
		private XElement FieldRef(string internalName)
		{
			return new XElement(Tags.FieldRef, new XAttribute(Tags.Name, internalName));
		}

		[NotNull]
		private XElement FieldRef([NotNull] FieldRefModel fieldRef)
		{
			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Name, GetFieldInternalName(fieldRef)));
		}

		[NotNull]
		private XElement FieldRef([NotNull] OrderByModel orderBy)
		{
			var ascending = orderBy.Ascending.ToString().ToUpper();

			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Name, GetFieldInternalName(orderBy.FieldRef)),
				new XAttribute(Tags.Ascending, ascending));
		}

		[NotNull]
		private XElement FieldRef(MetaField metaField)
		{
			var isLookup = metaField.TypeAsString.StartsWith("User") || metaField.TypeAsString.StartsWith("Lookup");

			return new XElement(Tags.FieldRef,
				new XAttribute(Tags.Name, metaField.InternalName),
				isLookup ? new XAttribute("LookupId", "TRUE") : null);
		}

		#endregion


		#region [Where Specials]

		[NotNull]
		private XElement WhereContainsOrIncludes([NotNull] ComparisonModel comparison)
		{
			if (comparison.Field.Type != FieldRefType.KnownMember)
			{
				throw new NotSupportedException("Unsupported FieldRefType value");
			}

			var memberRef = (MemberRefModel) comparison.Field;
			var metaField = GetMetaField(memberRef);
			var isLookup = metaField.TypeAsString.StartsWith("User") || metaField.TypeAsString.StartsWith("Lookup");

			if (comparison.ComparisonOperator == ComparisonOperator.ContainsOrIncludes)
			{
				return WhereComparison(isLookup ? ComparisonOperator.Includes : ComparisonOperator.Contains,
					metaField,
					comparison.Value,
					comparison.IsValueConverted);
			}

			if (isLookup)
			{
				return WhereComparison(ComparisonOperator.NotIncludes, metaField, comparison.Value, comparison.IsValueConverted);
			}

			throw new NotSupportedException("Cannot negate Contains operation for non-lookup fields");
		}

		[NotNull]
		private XElement WhereComparison(ComparisonOperator comparisonOperator, [NotNull] FieldRefModel fieldRef, object value,
			bool alreadyConverted = false)
		{
			if (fieldRef.Type != FieldRefType.KnownMember)
			{
				return Where(comparisonOperator,
					FieldRef(fieldRef),
					Value(fieldRef, value, alreadyConverted));
			}

			return WhereComparison(comparisonOperator, GetMetaField((MemberRefModel) fieldRef), value, alreadyConverted);
		}

		[NotNull]
		private XElement WhereComparison(ComparisonOperator comparisonOperator, [NotNull] MetaField metaField, object value,
			bool alreadyConverted = false)
		{
			return Where(comparisonOperator, FieldRef(metaField), Value(metaField, value, alreadyConverted));
		}

		#endregion


		#region [Value]

		[NotNull]
		private XElement Value([NotNull] FieldRefModel fieldRef, [CanBeNull] object value, bool alreadyConverted = false)
		{
			if (fieldRef.Type == FieldRefType.KnownMember)
			{
				return Value(GetMetaField((MemberRefModel) fieldRef), value, alreadyConverted);
			}

			if (!alreadyConverted)
			{
				throw new NotSupportedException("Only already converted values allowed with non FieldRefType.KnownMember field refs");
			}

			return new XElement(Tags.Value, value);
		}

		[NotNull]
		private XElement Value([NotNull] MetaField metaField, [CanBeNull] object value, bool alreadyConverted = false)
		{
			var camlValue = alreadyConverted
				? value
				: GetConverter(metaField).ToCamlValue(value);
			var typeAttr = metaField.IsCalculated
				? new XAttribute(Tags.Type, metaField.OutputType)
				: new XAttribute(Tags.Type, metaField.TypeAsString);
			var includeTimeAttr = metaField.TypeAsString == "DateTime"
				? new XAttribute("IncludeTimeValue", "TRUE")
				: null;

			return new XElement(Tags.Value, typeAttr, includeTimeAttr, camlValue);
		}

		#endregion


		#region [Helpers]

		[NotNull]
		private string GetFieldInternalName([NotNull] FieldRefModel fieldRef)
		{
			switch (fieldRef.Type)
			{
				case FieldRefType.Key:
					return ContentType.List.IsExternal ? Fields.BdcIdentity : Fields.Id;
				case FieldRefType.ContentTypeId:
					return Fields.ContentTypeId;
				case FieldRefType.KnownMember:
					return GetMetaField((MemberRefModel) fieldRef).InternalName;
			}

			throw new NotSupportedException("Unsupported FieldRefType value");
		}


		[NotNull]
		private MetaField GetMetaField([NotNull] MemberRefModel memberRef)
		{
			var member = memberRef.Member;
			if (!ContentType.Fields.ContainsKey(member.Name))
			{
				throw new InvalidOperationException(string.Format("'{0}' wasn't mapped and cannot be used in CAML query.", member));
			}

			return ContentType.Fields[member.Name];
		}

		private IFieldConverter GetConverter([NotNull] MetaField field)
		{
			var converter = field.Converter;
			if (converter == null)
			{
				throw new InvalidOperationException(string.Format("Converter wasn't initialized for '{0}' field", field.MemberName));
			}
			return converter;
		}

		#endregion

	}
}