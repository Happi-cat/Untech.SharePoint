using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators.ExpressionVisitors
{
	internal class Evaluator : ExpressionVisitor
	{
		protected Nominator Nominator { get; private set; }

		public Evaluator()
		{
			Nominator = new Nominator();
		}

		public Evaluator(Nominator nominator)
		{
			if (nominator == null) 
				throw new ArgumentNullException("nominator");

			Nominator = nominator;
		}

		public override Expression Visit(Expression node)
		{
			Nominator.Reset();
			Nominator.Visit(node);
			return new SubtreeEvaluator(Nominator.Candidates).Visit(node);
		}

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
}