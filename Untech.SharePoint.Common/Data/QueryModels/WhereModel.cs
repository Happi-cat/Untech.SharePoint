using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	public abstract class WhereModel
	{
		public static WhereModel And(WhereModel left, WhereModel right)
		{
			if (left == null)
				return right;
			if (right != null)
				return new LogicalJoinModel(LogicalJoinOperator.And, left, right);
			return left;
		}

		public static WhereModel Or(WhereModel left, WhereModel right)
		{
			if (left == null)
				return right;
			if (right != null)
				return new LogicalJoinModel(LogicalJoinOperator.Or, left, right);
			return left;
		}

		public static WhereModel In<T>(FieldRefModel field, IEnumerable<T> values)
		{
			return values.Aggregate<T, WhereModel>(null, (where, value) =>
			{
				var spDataComparison = new ComparisonModel(ComparisonOperator.Eq, field, value);
				return Or(@where, spDataComparison);
			});
		}

		public abstract WhereModel Negate();
	}
}