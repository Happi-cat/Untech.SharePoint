using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Describes different types of comparison operations allowed in CAML.
	/// </summary>
	[PublicAPI]
	public enum ComparisonOperator
	{
		/// <summary>
		/// Not specified.
		/// </summary>
		None,
		/// <summary>
		/// String begins with.
		/// </summary>
		BeginsWith,
		/// <summary>
		/// String contains.
		/// </summary>
		Contains,
		/// <summary>
		/// Equals.
		/// </summary>
		Eq,
		/// <summary>
		/// Greater than or equals.
		/// </summary>
		Geq,
		/// <summary>
		/// Greater than.
		/// </summary>
		Gt,
		/// <summary>
		/// Less than or equals.
		/// </summary>
		Leq,
		/// <summary>
		/// Less than.
		/// </summary>
		Lt,
		/// <summary>
		/// Not equals.
		/// </summary>
		Neq,
		/// <summary>
		/// Not equals to null.
		/// </summary>
		IsNotNull,
		/// <summary>
		/// Equals to null.
		/// </summary>
		IsNull,
		/// <summary>
		/// Lookup includes value.
		/// </summary>
		Includes,
		/// <summary>
		/// Lookup not includes value.
		/// </summary>
		NotIncludes,
		/// <summary>
		/// Pseudo-operator that will be replaced with <see cref="Contains"/> or <see cref="Includes"/> according to specific parameters.
		/// </summary>
		ContainsOrIncludes,
		/// <summary>
		/// Pseudo-operator that will be replaced with <see cref="NotIncludes"/> according to specific parameters.
		/// </summary>
		NotContainsOrIncludes
	}
}