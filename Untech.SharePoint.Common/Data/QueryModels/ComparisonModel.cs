using System;
using System.Collections.Generic;

namespace Untech.SharePoint.Common.Data.QueryModels
{
	public class ComparisonModel : WhereModel
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
			{ComparisonOperator.IsNull, ComparisonOperator.IsNotNull}
		};

		public ComparisonModel(ComparisonOperator comparisonOperator, FieldRefModel field, object value)
		{
			ComparisonOperator = comparisonOperator;
			Field = field;
			Value = value;
		}

		public ComparisonOperator ComparisonOperator { get; set; }

		public FieldRefModel Field { get; set; }

		public object Value { get; set; }

		public override WhereModel Negate()
		{
			if (NegateMap.ContainsKey(ComparisonOperator))
			{
				return new ComparisonModel(NegateMap[ComparisonOperator], Field, Value);
			}
			throw new NotSupportedException();
		}

		public override string ToString()
		{
			var valueString = "";
			if (ComparisonOperator != ComparisonOperator.IsNull && ComparisonOperator != ComparisonOperator.IsNotNull)
			{
				valueString = string.Format("<Value>{0}</Value>", Convert.ToString(Value));
			}

			return string.Format("<{0}>{1}{2}</{0}>", ComparisonOperator, Field, valueString);
		}
	}
}