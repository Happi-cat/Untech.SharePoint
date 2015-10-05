using System;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.Translators.Predicate;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	public abstract class BaseExpressionVisitorTest
	{
		protected abstract ExpressionVisitor Visitor { get; }

		protected virtual void Test(Expression<Func<VisitorsTestClass, bool>> original,
			Expression<Func<VisitorsTestClass, bool>> expected)
		{
			var visitors = new[] {Visitor};

			CustomAssert.AreEqualAfterVisit(visitors, original, expected);
		}

		protected virtual void TestWitEvaluator(Expression<Func<VisitorsTestClass, bool>> original, Expression<Func<VisitorsTestClass, bool>> expected)
		{
			var visitors = new[] { new Evaluator(),  Visitor };

			CustomAssert.AreEqualAfterVisit(visitors, original, expected);
		}
	}
}