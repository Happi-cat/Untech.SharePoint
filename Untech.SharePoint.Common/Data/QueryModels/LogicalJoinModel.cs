using System;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.QueryModels
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
		{
			Guard.CheckNotNull("first", first);
			Guard.CheckNotNull("second", second);

			LogicalOperator = logicalOperator;
			First = first;
			Second = second;
		}

		/// <summary>
		/// Gets CAML logical operator type.
		/// </summary>
		public LogicalJoinOperator LogicalOperator { get; private set; }

		/// <summary>
		/// Gets first operand.
		/// </summary>
		[NotNull]
		public WhereModel First { get; private set; }

		/// <summary>
		/// Gets second operand.
		/// </summary>
		[NotNull]
		public WhereModel Second { get; private set; }


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