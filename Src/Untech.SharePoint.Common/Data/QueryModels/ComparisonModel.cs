using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Represents CAML comparison tags, like Eq, Neq and etc.
	/// </summary>
	public sealed class ComparisonModel : WhereModel
	{
		private static readonly Dictionary<ComparisonOperator, ComparisonOperator> NegateMap = new Dictionary
			<ComparisonOperator, ComparisonOperator>
		{
			{ComparisonOperator.Eq, ComparisonOperator.Neq},
			{ComparisonOperator.Geq, ComparisonOperator.Lt},
			{ComparisonOperator.Gt, ComparisonOperator.Leq},
			{ComparisonOperator.Leq, ComparisonOperator.Gt},
			{ComparisonOperator.Lt, ComparisonOperator.Geq},
			{ComparisonOperator.Neq, ComparisonOperator.Eq},
			{ComparisonOperator.IsNotNull, ComparisonOperator.IsNull},
			{ComparisonOperator.IsNull, ComparisonOperator.IsNotNull},
			{ComparisonOperator.Includes, ComparisonOperator.NotIncludes},
			{ComparisonOperator.NotIncludes, ComparisonOperator.Includes},
			{ComparisonOperator.ContainsOrIncludes, ComparisonOperator.NotContainsOrIncludes},
			{ComparisonOperator.NotContainsOrIncludes, ComparisonOperator.ContainsOrIncludes}
		};

		/// <summary>
		/// Initializes a new instance of the <see cref="ComparisonModel"/>
		/// </summary>
		/// <param name="comparisonOperator">Comparison operator.</param>
		/// <param name="field">Comparable FieldRef model.</param>
		/// <param name="value">Value that is expected. Can be null.</param>
		/// <exception cref="ArgumentNullException"><paramref name="field"/> is null.</exception>
		public ComparisonModel(ComparisonOperator comparisonOperator, [NotNull]FieldRefModel field, [CanBeNull]object value)
			: base (WhereType.Comparison)
		{
			Guard.CheckNotNull(nameof(field), field);
			
			ComparisonOperator = comparisonOperator;
			Field = field;
			Value = value;
		}

		/// <summary>
		/// Gets the operator type of CAML comparison operation.
		/// </summary>
		public ComparisonOperator ComparisonOperator { get; }

		/// <summary>
		/// Gets comparable FieldRef.
		/// </summary>
		[NotNull]
		public FieldRefModel Field { get; }

		/// <summary>
		/// Determines whether <see cref="Value"/> is already converted to CAML string.
		/// </summary>
		public bool IsValueConverted { get; set; }

		/// <summary>
		/// Gets comparable value.
		/// </summary>
		[CanBeNull]
		public object Value { get; }

		/// <summary>
		/// Returns inverted comparison.
		/// </summary>
		/// <returns>Inverted comparison.</returns>
		/// <exception cref="NotSupportedException">Cannot invert current comparison.</exception>
		[NotNull]
		public override WhereModel Negate()
		{
			if (NegateMap.ContainsKey(ComparisonOperator))
			{
				return new ComparisonModel(NegateMap[ComparisonOperator], Field, Value);
			}
			throw new NotSupportedException($"Unable to negate: {ComparisonOperator}");
		}

		/// <summary>
		/// Returns a <see cref="String"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
		public override string ToString()
		{
			var valueString = "";
			if (ComparisonOperator != ComparisonOperator.IsNull && ComparisonOperator != ComparisonOperator.IsNotNull)
			{
				valueString = $"<Value>{Convert.ToString(Value)}</Value>";
			}

			return string.Format("<{0}>{1}{2}</{0}>", ComparisonOperator, Field, valueString);
		}
	}
}