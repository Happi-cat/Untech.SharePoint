using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.Translators.Predicate;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	public abstract class BaseExpressionVisitorTest : BaseExpressionTest
	{
		public class TestScenario
		{
			private readonly BaseExpressionVisitorTest _parent;
			private readonly Expression<Func<Entity, bool>> _given;
			private readonly List<ExpressionVisitor> _preVisitors;
			private readonly List<ExpressionVisitor> _postVisitors;

			public TestScenario(BaseExpressionVisitorTest parent, Expression<Func<Entity, bool>> given)
			{
				_parent = parent;
				_given = given;
				_preVisitors = new List<ExpressionVisitor>();
				_postVisitors = new List<ExpressionVisitor>();
			}

			public TestScenario PreEvaluate()
			{
				return PreVisit(new Evaluator());
			}

			public TestScenario PreVisit(ExpressionVisitor visitor)
			{
				_preVisitors.Add(visitor);

				return this;
			}

			public TestScenario PostEvaluate()
			{
				return PostVisit(new Evaluator());
			}


			public TestScenario PostVisit(ExpressionVisitor visitor)
			{
				_postVisitors.Add(visitor);

				return this;
			}

			public void Expected(Expression<Func<Entity, bool>> expected)
			{
				var visitors = _preVisitors
					.Concat(new[] { _parent.Visitor })
					.Concat(_postVisitors);

				CustomAssert.AreEqualAfterVisit(visitors, _given, expected);
			}
		}

		protected abstract ExpressionVisitor Visitor { get; }

		protected TestScenario Given(Expression<Func<Entity, bool>> given)
		{
			return new TestScenario(this, given);
		}
	}
}