using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.Translators.NegateRules;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.ExpressionVisitors
{
	internal class PushNotDownVisitor : ExpressionVisitor
	{
		public PushNotDownVisitor()
		{
			NegateRules = new List<INegateRule>
			{
				new ComparisonNegateRule(),
				new LogicalJoinNegateRule(),
				new NotNegateRule(),
				new BoolConstNegateRule()
			};
		}

		protected IReadOnlyCollection<INegateRule> NegateRules { get; set; }

		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.Method != null || node.NodeType != ExpressionType.Not)
			{
				return base.VisitUnary(node);
			}

			var operand = node.Operand.StripQuotes();

			var rule = NegateRules.FirstOrDefault(n => n.CanNegate(operand));
			return rule != null ? Visit(rule.Negate(operand)) : base.VisitUnary(node);
		}

		#region [Negate Rules]

		#endregion

	}
}