namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class LogicalJoinModel : WhereModel
	{
		public LogicalJoinModel(LogicalJoinOperator logicalOperator, WhereModel first, WhereModel second)
		{
			LogicalOperator = logicalOperator;
			First = first;
			Second = second;
		}

		public LogicalJoinOperator LogicalOperator { get; set; }

		public WhereModel First { get; set; }

		public WhereModel Second { get; set; }

		public override WhereModel Negate()
		{
			var negativeOperator = LogicalOperator == LogicalJoinOperator.And ? LogicalJoinOperator.Or : LogicalJoinOperator.And;

			return new LogicalJoinModel(negativeOperator, First.Negate(), Second.Negate());
		}
	}
}