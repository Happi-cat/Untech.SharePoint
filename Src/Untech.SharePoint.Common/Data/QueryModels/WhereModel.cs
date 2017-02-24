using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Represents base class for CAML logical and comparison tags.
	/// </summary>
	[PublicAPI]
	public abstract class WhereModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WhereModel"/> with the specified <see cref="WhereType"/>
		/// </summary>
		/// <param name="whereType">Current <see cref="WhereType"/> type.</param>
		protected WhereModel(WhereType whereType)
		{
			Type = whereType;
		}

		/// <summary>
		/// Gets 'where' model type.
		/// </summary>
		public WhereType Type { get; }

		/// <summary>
		/// Creates logical 'And' operation between operands.
		/// </summary>
		/// <param name="left">Left operand. Can be null.</param>
		/// <param name="right">Right operand. Can be null.</param>
		/// <returns>Logical or comparison operator.</returns>
		[ContractAnnotation("left:null, right:null => null")]
		public static WhereModel And([CanBeNull]WhereModel left, [CanBeNull]WhereModel right)
		{
			if (left == null)
				return right;
			if (right == null)
				return left;
			return new LogicalJoinModel(LogicalJoinOperator.And, left, right);
		}

		/// <summary>
		/// Creates logical 'Or' operation between operands.
		/// </summary>
		/// <param name="left">Left operand. Can be null.</param>
		/// <param name="right">Right operand. Can be null.</param>
		/// <returns>Logical or comparison operator.</returns>
		[ContractAnnotation("left:null, right:null => null")]
		public static WhereModel Or([CanBeNull]WhereModel left, [CanBeNull]WhereModel right)
		{
			if (left == null)
				return right;
			if (right == null)
				return left;
			return new LogicalJoinModel(LogicalJoinOperator.Or, left, right);
		}

		/// <summary>
		/// Creates 'in' operation for the specified FieldRef.
		/// </summary>
		/// <param name="field">FieldRef that should be compared.</param>
		/// <param name="values">Values to compare with.</param>
		/// <returns>New comparison or null.</returns>
		[CanBeNull]
		public static WhereModel In([NotNull]FieldRefModel field, [CanBeNull]IEnumerable<object> values)
		{
			values = values ?? new object[0];
			return values.Aggregate<object, WhereModel>(null, (where, value) =>
			{
				var spDataComparison = new ComparisonModel(ComparisonOperator.Eq, field, value);
				return Or(@where, spDataComparison);
			});
		}

		/// <summary>
		/// Returns inverted comparison.
		/// </summary>
		/// <returns>Inverted comparison.</returns>
		public abstract WhereModel Negate();
	}
}