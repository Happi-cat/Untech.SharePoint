namespace Untech.SharePoint.Common.Data.QueryModels
{
	/// <summary>
	/// Describes different types of <see cref="WhereModel"/>
	/// </summary>
	public enum WhereType
	{
		/// <summary>
		/// Equals to <see cref="LogicalJoinModel"/>
		/// </summary>
		LogicalJoin,
		/// <summary>
		/// Equals to <see cref="ComparisonModel"/>
		/// </summary>
		Comparison
	}
}