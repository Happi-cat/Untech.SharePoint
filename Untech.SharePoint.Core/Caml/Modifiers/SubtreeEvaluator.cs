using System.Collections.Generic;
using System.Linq.Expressions;

namespace Untech.SharePoint.Core.Caml.Modifiers
{
	internal class SubtreeEvaluator : ExpressionVisitor
	{
		private readonly HashSet<Expression> _candidates;

		internal SubtreeEvaluator(HashSet<Expression> candidates)
		{
			_candidates = candidates;
		}

		public override Expression Visit(Expression exp)
		{
			if (exp == null)
			{
				return null;
			}

			return _candidates.Contains(exp) ? Evaluate(exp) : base.Visit(exp);
		}

		private static Expression Evaluate(Expression e)
		{
			if (e.NodeType == ExpressionType.Constant)
			{
				return e;
			}

			var func = Expression.Lambda(e).Compile();
			return Expression.Constant(func.DynamicInvoke(null), e.Type);
		}
	}
}