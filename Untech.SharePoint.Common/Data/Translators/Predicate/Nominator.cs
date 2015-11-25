using System.Collections.Generic;
using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal sealed class Nominator : ExpressionVisitor
	{
		internal Nominator()
		{
			Candidates = new HashSet<Expression>();
		}

		public HashSet<Expression> Candidates { get; private set; }
		private bool CanBeEvaluated { get; set; }

		public void Reset()
		{
			Candidates = new HashSet<Expression>();
		}

		public override Expression Visit(Expression expression)
		{
			if (expression == null)
			{
				return null;
			}

			var previousCanBeEvaluated = CanBeEvaluated;
			CanBeEvaluated = true;
			base.Visit(expression);
			if (CanBeEvaluated)
			{
				if (CanEvaluate(expression))
				{
					Candidates.Add(expression);
				}
				else
				{
					CanBeEvaluated = false;
				}
			}
			CanBeEvaluated &= previousCanBeEvaluated;
			return expression;
		}

		private bool CanEvaluate(Expression node)
		{
			return node.NodeType != ExpressionType.Parameter;
		}
	}
}