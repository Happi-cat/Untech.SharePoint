using System;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Data.QueryModels
{
	/// <summary>
	/// Represents CAML logical tags, like And, Or.
	/// </summary>
	public sealed class LogicalJoinModel : WhereModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalJoinModel"/>.
		/// </summary>
		/// <param name="logicalOperator">Logical operator.</param>
		/// <param name="first">First operand.</param>
		/// <param name="second">Second operand.</param>
		/// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
		public LogicalJoinModel(LogicalJoinOperator logicalOperator, [NotNull]WhereModel first, [NotNull]WhereModel second)
			: base(WhereType.LogicalJoin)
		{
			Guard.CheckNotNull(nameof(first), first);
			Guard.CheckNotNull(nameof(second), second);

			LogicalOperator = logicalOperator;
			First = first;
			Second = second;
		}

		/// <summary>
		/// Gets CAML logical operator type.
		/// </summary>
		public LogicalJoinOperator LogicalOperator { get; }

		/// <summary>
		/// Gets first operand.
		/// </summary>
		[NotNull]
		public WhereModel First { get; }

		/// <summary>
		/// Gets second operand.
		/// </summary>
		[NotNull]
		public WhereModel Second { get; }

		/// <summary>
		/// Returns inverted comparison.
		/// </summary>
		/// <returns>Inverted comparison.</returns>
		[NotNull]
		public override WhereModel Negate()
		{
			var negativeOperator = LogicalOperator == LogicalJoinOperator.And ? LogicalJoinOperator.Or : LogicalJoinOperator.And;

			return new LogicalJoinModel(negativeOperator, First.Negate(), Second.Negate());
		}

		/// <summary>
		/// Returns a <see cref="string"/> which represents the object instance.
		/// </summary>
		/// <returns>CAML-like string.</returns>
		public override string ToString()
		{
			return string.Format("<{0}>{1}{2}</{0}>", LogicalOperator, First, Second);
		}
	}
}