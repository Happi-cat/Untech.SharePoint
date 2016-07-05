using System.Collections.Generic;
using System.Linq.Expressions;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class Evaluator : ExpressionVisitor
	{
		public Evaluator()
		{
			Nominator = new Nominator();
		}

		[NotNull]
		private Nominator Nominator { get; }

		public override Expression Visit(Expression node)
		{
			Nominator.Reset();
			Nominator.Visit(node);
			return new SubtreeEvaluator(Nominator.Candidates).Visit(node);
		}

		private class SubtreeEvaluator : ExpressionVisitor
		{
			[NotNull]
			private readonly HashSet<Expression> _candidates;

			internal SubtreeEvaluator([NotNull]HashSet<Expression> candidates)
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
}