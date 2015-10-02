using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Test.Data.Translators;

namespace Untech.SharePoint.Common.Test
{
	public static class CustomAssert
	{
		public static void Throw<TException>(Action action)
			where TException: Exception
		{
			var thrown = false;
			try
			{
				action();
			}
			catch (TException)
			{
				thrown = true;
			}
			Assert.IsTrue(thrown);
		}

		public static void AreEqualAfterVisit(IEnumerable<ExpressionVisitor> visitors, Expression<Func<VisitorsTestClass, bool>> original, Expression<Func<VisitorsTestClass, bool>> expected)
		{
			var processed = visitors.Aggregate((Expression)original, (expr, visitor) => visitor.Visit(expr));
			
			Assert.AreEqual(expected.ToString(), processed.ToString());
		}
	}
}