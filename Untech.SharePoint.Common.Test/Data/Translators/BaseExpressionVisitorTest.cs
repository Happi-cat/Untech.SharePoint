using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	public class BaseExpressionVisitorTest<T>
		where T: ExpressionVisitor, new()
	{
		protected void Test(Expression<Func<VisitorsTestClass, bool>> original, Expression<Func<VisitorsTestClass, bool>> expected)
		{
			Assert.AreEqual(expected.ToString(), new T().Visit(original).ToString());
		}
	}
}