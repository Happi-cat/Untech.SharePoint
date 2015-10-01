using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Data.Translators.ExpressionVisitors;
using Untech.SharePoint.Common.Test.Data.Translators.ExpressionVisitors;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	public class BaseExpressionVisitorTest<T>
		where T: ExpressionVisitor, new()
	{
		protected void Test(Expression<Func<VisitorsTestClass, bool>> original, Expression<Func<VisitorsTestClass, bool>> expected)
		{
			Assert.AreEqual(expected.ToString(), new T().Visit(original).ToString());
		}

		protected void TestWitEvaluator(Expression<Func<VisitorsTestClass, bool>> original, Expression<Func<VisitorsTestClass, bool>> expected)
		{
			var visitors = new ExpressionVisitor[] { new Evaluator(), new T() };


			Assert.AreEqual(expected.ToString(), visitors.Aggregate((Expression)original, (expr, visitor) => visitor.Visit(expr)).ToString());
		}
	}
}