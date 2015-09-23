namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class LogicalJoinModel : WhereModel
	{
		public LogicalJoinModel(LogicalJoinOperator logicalOperator, WhereModel left, WhereModel right)
		{
			
		}

		public LogicalJoinOperator LogicalOperator { get; set; }

		public WhereModel First { get; set; }

		public WhereModel Second { get; set; }

		public override WhereModel Negate()
		{
			var first = First.Negate();
			var second = Second.Negate();

			return new LogicalJoinModel(LogicalOperator == LogicalJoinOperator.And ? LogicalJoinOperator.Or : LogicalJoinOperator.And, first, second);
		}
	}
}