﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data.Translators.Predicate;

namespace Untech.SharePoint.Data.Translators
{
	public abstract class BaseExpressionVisitorTest : BaseExpressionTest
	{
		protected abstract ExpressionVisitor TestableVisitor { get; }

		protected TestScenario Given(Expression<Func<Entity, bool>> given)
		{
			return new TestScenario(this, given);
		}

		[PublicAPI]
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
					.Concat(new[] { _parent.TestableVisitor })
					.Concat(_postVisitors);

				var processed = visitors.Aggregate((Expression)_given, (expr, visitor) => visitor.Visit(expr));

				Assert.AreEqual(expected.ToString(), processed.ToString());
			}
		}
	}
}