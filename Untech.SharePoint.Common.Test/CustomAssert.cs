using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Data.Translators;

namespace Untech.SharePoint.Common.Test
{
	public static class CustomAssert
	{
		public static void Throw<TException>(Action action)
			where TException: Exception
		{
			try
			{
				action();
			}
			catch (TException)
			{
				return;
			}
			Assert.Fail("Exception '{0}' wasn't thrown.", typeof(TException));
		}

		public static void AreEqualAfterVisit(IEnumerable<ExpressionVisitor> visitors, Expression<Func<VisitorsTestClass, bool>> original, Expression<Func<VisitorsTestClass, bool>> expected)
		{
			var processed = visitors.Aggregate((Expression)original, (expr, visitor) => visitor.Visit(expr));
			
			Assert.AreEqual(expected.ToString(), processed.ToString());
		}
	}
}